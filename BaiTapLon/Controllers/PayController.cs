using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace BaiTapLon.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        Model1 db = new Model1();
        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["nguoidung"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Lấy tài khoản từ session
            taikhoan taikhoan = Session["nguoidung"] as taikhoan;


            // Lấy danh sách sản phẩm trong giỏ hàng của người dùng hiện tại
            var listProduct = new Model1().giohangs.Where(item => item.tk == taikhoan.tk).ToList();
            return View(listProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string RadioGroup1, string RadioGroup2, FormCollection collection)
        {
            string submitButton = collection["submitButton"];
            if (submitButton == "Pay")
            {
                taikhoan taikhoan = Session["nguoidung"] as taikhoan;
                var listProduct = new Model1().giohangs.Where(item => item.tk == taikhoan.tk).ToList();

                //Get Config Info
                string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
                string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

                //Get payment input
                OrderInfo order = new OrderInfo();
                order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                order.Amount = listProduct.Sum(item => (item.sanpham.Gia ?? 0) * (item.SoLuong ?? 0));
                // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
                order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
                order.CreatedDate = DateTime.Now;
                //Save order to db

                //Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();

                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (order.Amount * 100 * 24985).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân) sau đó nhân với 24985(tỉ giá tiên đô), sau đó gửi sang VNPAY
                if (RadioGroup1 == "bankcode_Vnpayqr")
                {
                    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
                }
                else if (RadioGroup1 == "bankcode_Vnbank")
                {
                    vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                }
                else if (RadioGroup1 == "bankcode_Intcard")
                {
                    vnpay.AddRequestData("vnp_BankCode", "INTCARD");
                }

                vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

                if (RadioGroup2 == "locale_Vn")
                {
                    vnpay.AddRequestData("vnp_Locale", "vn");
                }
                else if (RadioGroup2 == "locale_En")
                {
                    vnpay.AddRequestData("vnp_Locale", "en");
                }
                vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
                vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

                //Add Params of 2.1.0 Version
                //Billing

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
                Response.Redirect(paymentUrl);
                return View();
            }
            return View();
        }
        public ActionResult PayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        taikhoan taikhoan = Session["nguoidung"] as taikhoan;
                        var listProduct = new Model1().giohangs.Where(item => item.tk == taikhoan.tk).ToList();
                        //Thanh toan thanh cong
                        ViewBag.displayMsg = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        // Thêm đơn hàng vào db
                        foreach(var item in listProduct)
                        {
                            string uniqueMaDH = GenerateUniqueMaDH();
                            donhang donhang = new donhang();

                            donhang.MaDH = uniqueMaDH;
                            donhang.ThoiGian = DateTime.Now;
                            donhang.tk = taikhoan.tk;
                            donhang.MaSP = item.MaSP;
                            donhang.SoLuong = item.SoLuong;
                            var tk = item.tk;
                            var diaChi = db.nguoidungs.Where(nd => nd.email == tk).Select(nd => nd.DiaChi).FirstOrDefault();
                            donhang.DiaChi = diaChi;

                            db.donhangs.Add(donhang);
                            db.SaveChanges();
                            //them dia chi vao sau luoi que
                        }    
                        
                        //Thuc hien chinh sua so luong trong db 
                        foreach (var item in listProduct)
                        {
                            // Cập nhật dữ liệu trong cơ sở dữ liệu sản phẩm
                            var model = db.sanphams.Find(item.MaSP);
                            if (model != null)
                            {
                                model.SoLuong -= item.SoLuong; // Giảm số lượng từ tồn kho
                            }
                        }
                        db.SaveChanges();

                        // Xoá toàn bộ sản phẩm trong giỏ hàng
                        // Lấy tài khoản từ session

                        // Xóa tất cả các mục trong giỏ hàng của người dùng hiện tại
                        var itemsToRemove = db.giohangs.Where(item => item.tk == taikhoan.tk);
                        db.giohangs.RemoveRange(itemsToRemove);

                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.displayMsg = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    ViewBag.displayTmnCode = "Mã Website (Terminal ID):" + TerminalID;
                    ViewBag.displayTxnRef = "Mã giao dịch thanh toán:" + orderId.ToString();
                    ViewBag.displayVnpayTranNo = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.displayAmount = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    ViewBag.displayBankCode = "Ngân hàng thanh toán:" + bankCode;
                }
                else
                {
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    ViewBag.displayMsg = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            return View();
        }
            // GET: Pay/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pay/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pay/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pay/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pay/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pay/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pay/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        private string GenerateUniqueMaDH()
        {
            string uniqueMaDH;

            do
            {
                uniqueMaDH = GenerateMaDH();
            } while (!IsMaDHUnique(uniqueMaDH));

            return uniqueMaDH;
        }

        private string GenerateMaDH()
        {
            // Ví dụ đơn giản: sử dụng GUID và chỉ lấy 8 ký tự đầu tiên
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        private bool IsMaDHUnique(string maDH)
        {
            return !db.donhangs.Any(u => u.MaDH == maDH);
        }
    }
}
