
using DO_AN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DO_AN

{
    class Vattu

    {
        //sử dụng các từ khóa public để khai báo các thuộc tính và phương thức
        //là public để có thể truy cập từ bất kỳ nơi nào trong chương trình.

        public string MAVT;
        public string TENVT;
        public string DVT;
        public int SoLuongTon;
        public Vattu left;
        public Vattu right;

        public Vattu(string mAVT, string tENVT, string dVT, int soLuongTon)
        {
            MAVT = mAVT;
            TENVT = tENVT;
            DVT = dVT;
            SoLuongTon = soLuongTon;
            left = null;
            right = null;
        }


    }


    class QLVT
    {
        public Vattu root; // nút gốc của cây nhị phân tìm kiếm danh sách vật tư

        // Hàm khởi tạo, tạo một cây rỗng
        public QLVT()
        {
            root = null;
        }


        // Hàm tìm kiếm vật tư theo mã vật tư

        public Vattu TimVT(string maVT)
        {
            Vattu current = root;
            while (current != null)
            {
                if (maVT == current.MAVT)
                {
                    return current;
                }
                else if (maVT.CompareTo(current.MAVT) < 0)
                {
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }
            return null;
        }

        // Hàm thêm một vật tư mới vào cây
        public void ThemVT()
        {
            Console.WriteLine("Nhap thong tin vat tu: ");
            Console.Write("Ma VT: ");
            string maVT = Console.ReadLine().ToUpper();
            // Kiểm tra mã VT đã tồn tại trong cây chưa
            // Kiểm tra mã VT đã tồn tại trong cây chưa
            Vattu existingVattu = TimVT(maVT);
            if (existingVattu != null)
            {
                Console.WriteLine("Mã VT đã tồn tại.");
                return;
            }
            string tenVT = string.Empty;
            while (string.IsNullOrWhiteSpace(tenVT))
            {
                Console.Write("Ten VT: ");
                tenVT = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(tenVT))
                {
                    Console.WriteLine("Tên VT không được để trống. Vui lòng nhập lại.");
                }
            }

            string dvt = string.Empty;
            while (string.IsNullOrWhiteSpace(dvt))
            {
                Console.Write("DVT: ");
                dvt = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(dvt))
                {
                    Console.WriteLine("DVT không được để trống. Vui lòng nhập lại.");
                }
            }

            int slTon;
            // Nếu vật tư chưa có trong danh sách, yêu cầu nhập số lượng tồn
            if (existingVattu == null)
            {
                Console.Write("So luong ton: ");
                slTon = int.Parse(Console.ReadLine());
            }
            else
            {
                slTon = TimVT(maVT).SoLuongTon;
            }

            Vattu vt = new Vattu(maVT, tenVT, dvt, slTon);
            root = ThemVT(root, vt);
            Console.WriteLine("Them vat tu thanh cong!");
        }

        // Hàm đệ quy thêm vật tư mới vào cây
        public Vattu ThemVT(Vattu node, Vattu vt)
        {
            if (node == null)
            {
                node = vt;
            }
            else if (string.Compare(vt.MAVT, node.MAVT) < 0)
            {
                node.left = ThemVT(node.left, vt);
            }
            else if (string.Compare(vt.MAVT, node.MAVT) > 0)
            {
                node.right = ThemVT(node.right, vt);
            }
            return node;
        }









        public void XoaVatTu(string maVT)
        {
            root = XoaVatTu(root, maVT);
        }

        // Phương thức đệ quy để xóa vật tư từ cây
        private Vattu XoaVatTu(Vattu node, string maVT)
        {
            // Nếu cây rỗng, không có gì để xóa
            if (node == null)
                return null;

            // Di chuyển đến vị trí nút chứa vật tư cần xóa
            int compareResult = maVT.CompareTo(node.MAVT);
            if (compareResult < 0)
            {
                node.left = XoaVatTu(node.left, maVT);
            }
            else if (compareResult > 0)
            {
                node.right = XoaVatTu(node.right, maVT);
            }
            else
            {
                // Đã tìm thấy vật tư cần xóa

                // Trường hợp 1: Nút cần xóa là nút lá hoặc chỉ có một con
                if (node.left == null)
                    return node.right;
                else if (node.right == null)
                    return node.left;

                // Trường hợp 2: Nút cần xóa có hai con
                // Tìm phần tử thay thế, tức là phần tử nhỏ nhất trong cây con phải
                Vattu minValueNode = TimMin(node.right);

                // Gán giá trị của phần tử nhỏ nhất vào nút cần xóa
                node.MAVT = minValueNode.MAVT;
                node.TENVT = minValueNode.TENVT;
                node.DVT = minValueNode.DVT;
                node.SoLuongTon = minValueNode.SoLuongTon;

                // Xóa phần tử nhỏ nhất từ cây con phải
                node.right = XoaVatTu(node.right, minValueNode.MAVT);
            }

            return node;
        }

        // Phương thức tìm phần tử nhỏ nhất trong cây con phải
        private Vattu TimMin(Vattu node)
        {
            Vattu current = node;

            // Lặp qua cây con trái để tìm phần tử nhỏ nhất
            while (current.left != null)
            {
                current = current.left;
            }

            return current;
        }

        // Các phương thức và thuộc tính khác
        // Hàm hiệu chỉnh thông tin của vật tư
        public void HieuChinhVatTu(string mavt, string tenvt, string dvt, int soluongton)
        {
            Vattu node = TimVT(mavt);

            if (node == null)
            {
                Console.WriteLine("Không tìm thấy vật tư có mã là " + mavt);
            }
            else
            {
                node.TENVT = tenvt;
                node.DVT = dvt;

                // Chỉ cho phép nhập số lượng tồn khi vật tư đó là mới
                if (node.SoLuongTon == 0)
                {
                    node.SoLuongTon = soluongton;
                }
            }
        }


        public void InDanhSachVatTuTonKho(Vattu root)
        {
            if (root != null)
            {
                InDanhSachVatTuTonKho(root.left);
                Console.WriteLine("|" + root.MAVT.PadRight(10) + root.TENVT.PadRight(30) + root.DVT.PadRight(10) + root.SoLuongTon.ToString().PadRight(15) + "|");
                InDanhSachVatTuTonKho(root.right);
            }
        }

        public void CapNhatSoLuongTon(string mavt, int soluong)
        {
            // Tìm vật tư có mã là mavt trong danh sách vật tư
            Vattu vt = TimVT(mavt);

            // Nếu vật tư tồn tại, cập nhật số lượng tồn mới
            if (vt != null)
            {
                vt.SoLuongTon -= soluong;
            }
            else
            {
                // Nếu không tìm thấy vật tư, có thể quăng ra một exception hoặc thông báo lỗi khác tùy vào yêu cầu của bài toán.
                //throw new Exception("Không tìm thấy vật tư có mã là " + mavt);
                Console.WriteLine("Không tìm thấy vật tư có mã là " + mavt);

            }
        }

        public int SoLuongTon(string mavt)
        {
            // Tìm vật tư có mã là mavt trong danh sách vật tư
            Vattu vt = TimVT(mavt);

            // Nếu vật tư tồn tại, trả về số lượng tồn của vật tư đó
            if (vt != null)
            {
                return vt.SoLuongTon;
            }
            else
            {
                // Trả về giá trị mặc định khi không tìm thấy vật tư
                return -1;
            }
        }


        // Hàm in danh sách vật tư tồn kho theo thứ tự tên vật tư tăng dần
        public void InDanhSachVatTuTonKho2()
        {
            Vattu[] danhSachVatTu = new Vattu[DemSoLuongVatTuTonKho(root)];
            int index = 0;
            InDanhSachVatTuTonKho(root, danhSachVatTu, ref index);

            SapXepTheoTenVatTu(danhSachVatTu);

            Console.WriteLine("Danh sách vật tư tồn kho:");
            Console.WriteLine("Mã VT\tTên vật tư\tĐơn vị tính\tSố lượng tồn");
            foreach (var vt in danhSachVatTu)
            {
                Console.WriteLine($"{vt.MAVT}\t{vt.TENVT}\t{vt.DVT}\t{vt.SoLuongTon}");
            }
        }

        // Hàm đệ quy duyệt cây và thêm vật tư vào mảng tạm thời
        private void InDanhSachVatTuTonKho(Vattu node, Vattu[] danhSachVatTu, ref int index)
        {
            if (node != null)
            {
                InDanhSachVatTuTonKho(node.left, danhSachVatTu, ref index);
                danhSachVatTu[index] = new Vattu(node.MAVT, node.TENVT, node.DVT, node.SoLuongTon);
                index++;
                InDanhSachVatTuTonKho(node.right, danhSachVatTu, ref index);
            }
        }

        // Hàm đệ quy đếm số lượng vật tư trong cây
        private int DemSoLuongVatTuTonKho(Vattu node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + DemSoLuongVatTuTonKho(node.left) + DemSoLuongVatTuTonKho(node.right);
        }

        // Hàm sắp xếp mảng danh sách vật tư theo tên vật tư tăng dần
        private void SapXepTheoTenVatTu(Vattu[] danhSachVatTu)
        {
            int n = danhSachVatTu.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (string.Compare(danhSachVatTu[i].TENVT, danhSachVatTu[j].TENVT) > 0)
                    {
                        Vattu temp = danhSachVatTu[i];
                        danhSachVatTu[i] = danhSachVatTu[j];
                        danhSachVatTu[j] = temp;
                    }
                }
            }
        }



    }


    class Nhanvien
    {
        public string MANV;
        public string HO;
        public string TEN;
        public string PHAI;
        public Hoadon_List DS_HOADON;
        public Nhanvien(string ma, string ho, string ten, string phai)
        {
            MANV = ma;
            HO = ho;
            TEN = ten;
            PHAI = phai;
            DS_HOADON = new Hoadon_List(); // Khởi tạo danh sách hóa đơn
        }


    }
    class DSNhanVien
    {
        public Nhanvien[] danhSachNV;
        public int soLuongNV;

        public DSNhanVien()
        {
            danhSachNV = new Nhanvien[500];
            soLuongNV = 0;
        }

        public void ThemNhanVien(Nhanvien nv)
        {
            danhSachNV[soLuongNV] = nv;
            soLuongNV++;
        }

        public Nhanvien TimNhanVien(string maNV)
        {
            for (int i = 0; i < soLuongNV; i++)
            {
                if (danhSachNV[i].MANV == maNV)
                {
                    return danhSachNV[i];
                }
            }
            return null;
        }

        public void NhapNhanVien(DSNhanVien dsNhanVien)
        {

            // Nhập thông tin nhân viên
            Console.WriteLine("Nhập thông tin nhân viên");
            while (true)
            {
                string maNV = string.Empty;
                bool isValid = false;

                while (!isValid)
                {
                    Console.Write("Nhập mã nhân viên: ");
                    maNV = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(maNV))
                    {
                        Console.WriteLine("Mã nhân viên không được để trống. Vui lòng nhập lại.");
                    }
                    else if (maNV.Length < 3 || maNV.Length > 10)
                    {
                        Console.WriteLine("Mã nhân viên phải có độ dài từ 3 đến 10 ký tự. Vui lòng nhập lại.");
                    }
                    else
                    {
                        isValid = true; // Điều kiện hợp lệ, thoát khỏi vòng lặp.
                    }
                }

                // Tiếp tục xử lý logic của bạn với giá trị mã nhân viên đã nhập đúng.


                // Kiểm tra mã số nhân viên đã tồn tại hay chưa
                bool maNVTonTai = false;
                foreach (Nhanvien tamnv in dsNhanVien.danhSachNV)
                {
                    if (tamnv != null && tamnv.MANV == maNV)
                    {
                        maNVTonTai = true;
                        break;
                    }
                }

                if (maNVTonTai)
                {
                    Console.WriteLine("Mã số nhân viên đã tồn tại. Vui lòng nhập lại.");
                    continue; // Quay lại vòng lặp để nhập lại thông tin
                }

                string ho = string.Empty;
                while (string.IsNullOrWhiteSpace(ho))
                {
                    Console.Write("Họ: ");
                    ho = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ho))
                    {
                        Console.WriteLine("Họ không được để trống. Vui lòng nhập lại.");
                    }
                }

                string ten = string.Empty;
                while (string.IsNullOrWhiteSpace(ten))
                {
                    Console.Write("Tên: ");
                    ten = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ten))
                    {
                        Console.WriteLine("Tên không được để trống. Vui lòng nhập lại.");
                    }
                }

                string phai = string.Empty;
                while (true)
                {
                    Console.Write("Phái (Nam/Nu): ");
                    phai = Console.ReadLine();
                    if (phai.Equals("Nam", StringComparison.OrdinalIgnoreCase))
                    {
                        // Phái là Nam
                        break;
                    }
                    else if (phai.Equals("Nu", StringComparison.OrdinalIgnoreCase))
                    {
                        // Phái là Nữ
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Phái không hợp lệ. Vui lòng chỉ nhập 'Nam' hoặc 'Nu'.");
                    }
                }
                //Kiểm tra thông tin
                if (maNV == "" || ho == "" || ten == "")
                {
                    Console.WriteLine("Thông tin không hợp lệ, vui lòng nhập lại!");
                    continue;
                }

                //Tạo đối tượng nhân viên mới
                Nhanvien nv = new Nhanvien(maNV, ho, ten, phai);
                dsNhanVien.ThemNhanVien(nv);

                string tiepTuc;
                do
                {
                    Console.Write("Nhập thêm nhân viên? (Y/N): ");
                    tiepTuc = Console.ReadLine().ToLower();

                    if (tiepTuc != "y" && tiepTuc != "n")
                    {
                        Console.WriteLine("Giá trị không hợp lệ! Vui lòng nhập lại.");
                    }
                } while (tiepTuc != "y" && tiepTuc != "n");


                if (tiepTuc.ToLower() == "n")
                {
                    break;
                }
            }

            Console.WriteLine("Them nhân viên thành công!");
        }

        public void ChinhSuaNhanVien(string maNV)
        {
            Nhanvien nhanVien = TimNhanVien(maNV);
            if (nhanVien == null)
            {
                Console.WriteLine("Không tìm thấy nhân viên có mã {0}.", maNV);
                return;
            }

            Console.WriteLine("Chỉnh sửa thông tin nhân viên có mã {0}:", maNV);

            string ho = string.Empty;
            while (string.IsNullOrWhiteSpace(ho))
            {
                Console.Write("Họ: ");
                ho = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ho))
                {
                    Console.WriteLine("Họ không được để trống. Vui lòng nhập lại.");
                }
            }

            string ten = string.Empty;
            while (string.IsNullOrWhiteSpace(ten))
            {
                Console.Write("Tên: ");
                ten = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ten))
                {
                    Console.WriteLine("Tên không được để trống. Vui lòng nhập lại.");
                }
            }

            // Cập nhật thông tin nhân viên
            nhanVien.HO = ho;
            nhanVien.TEN = ten;

            Console.WriteLine("Thông tin nhân viên đã được chỉnh sửa thành công.");
        }


        public void TimKiemNhanVien(DSNhanVien dsNhanVien)
        {
            // Tìm kiếm nhân viên dựa trên mã nhân viên
            Console.WriteLine("Tìm kiếm nhân viên");
            while (true)
            {
                string maNV = string.Empty;
                bool isValid = false;

                while (!isValid)
                {
                    Console.Write("Nhập mã nhân viên: ");
                    maNV = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(maNV))
                    {
                        Console.WriteLine("Mã nhân viên không được để trống. Vui lòng nhập lại.");
                    }
                    else if (maNV.Length < 3 || maNV.Length > 10)
                    {
                        Console.WriteLine("Mã nhân viên phải có độ dài từ 3 đến 10 ký tự. Vui lòng nhập lại.");
                    }
                    else
                    {
                        isValid = true; // Điều kiện hợp lệ, thoát khỏi vòng lặp.
                    }
                }

                // Tiếp tục xử lý logic của bạn với giá trị mã nhân viên đã nhập đúng.

                // Tìm kiếm nhân viên
                Nhanvien nv = dsNhanVien.TimNhanVien(maNV);

                //Kiểm tra kết quả tìm kiếm
                if (nv == null)
                {
                    Console.WriteLine("Không tìm thấy nhân viên có mã {0}", maNV);
                }
                else
                {
                    Console.WriteLine("Mã nhân viên: {0}", nv.MANV);
                    Console.WriteLine("Họ tên: {0} {1}", nv.HO, nv.TEN);
                    Console.WriteLine("Phái: {0}", nv.PHAI);
                }

                string tiepTuc;
                do
                {
                    Console.Write("Tìm kiếm nhân viên? (Y/N): ");
                    tiepTuc = Console.ReadLine().ToLower();

                    if (tiepTuc != "y" && tiepTuc != "n")
                    {
                        Console.WriteLine("Giá trị không hợp lệ! Vui lòng nhập lại.");
                    }
                } while (tiepTuc != "y" && tiepTuc != "n");


                if (tiepTuc.ToLower() == "n")
                {
                    break;
                }
            }

        }
        public void InDanhSachNhanVienTheoThuTuTen(DSNhanVien dsNhanVien)
        {
            // Sắp xếp danh sách nhân viên theo tên tăng dần
            for (int i = 0; i < dsNhanVien.soLuongNV - 1; i++)
            {
                for (int j = 0; j < dsNhanVien.soLuongNV - i - 1; j++)
                {
                    if (SoSanhTen(dsNhanVien.danhSachNV[j], dsNhanVien.danhSachNV[j + 1]) > 0)
                    {
                        HoanViNhanVien(ref dsNhanVien.danhSachNV[j], ref dsNhanVien.danhSachNV[j + 1]);
                    }
                }
            }

            // In danh sách nhân viên
            Console.WriteLine();
            Console.WriteLine(" ********* Danh sách nhân viên theo thứ tự tên tăng dần *************** ");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("| Mã NV    | Họ và tên                 | Phái   |");
            Console.WriteLine("-------------------------------------------------");
            for (int i = 0; i < dsNhanVien.soLuongNV; i++)
            {
                Nhanvien nv = dsNhanVien.danhSachNV[i];
                Console.WriteLine("| {0,-8} | {1,-25} | {2,-6} |", nv.MANV, (nv.HO + " " + nv.TEN), nv.PHAI);
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
        }

        public int SoSanhTen(Nhanvien nv1, Nhanvien nv2)
        {
            // Lấy độ dài của tên ngắn nhất trong hai nhân viên
            int minLength = Math.Min(nv1.TEN.Length, nv2.TEN.Length);

            // So sánh từng ký tự của tên trong hai nhân viên
            for (int i = 0; i < minLength; i++)
            {
                if (nv1.TEN[i] < nv2.TEN[i])
                {
                    return -1; // Tên của nhân viên nv1 nhỏ hơn tên của nhân viên nv2
                }
                else if (nv1.TEN[i] > nv2.TEN[i])
                {
                    return 1; // Tên của nhân viên nv1 lớn hơn tên của nhân viên nv2
                }
            }

            // Nếu tên của hai nhân viên giống nhau đến độ dài ngắn nhất,
            // so sánh theo độ dài của tên để xác định nhân viên có tên dài hơn
            if (nv1.TEN.Length < nv2.TEN.Length)
            {
                return -1; // Tên của nhân viên nv1 nhỏ hơn tên của nhân viên nv2
            }
            else if (nv1.TEN.Length > nv2.TEN.Length)
            {
                return 1; // Tên của nhân viên nv1 lớn hơn tên của nhân viên nv2
            }

            return 0; // Tên của hai nhân viên bằng nhau
        }

        public void HoanViNhanVien(ref Nhanvien nv1, ref Nhanvien nv2)
        {
            Nhanvien temp = nv1;
            nv1 = nv2;
            nv2 = temp;
        }



    }



    class Hoadon
    {
        public string SoHD;         // Số hóa đơn
        public DateTime NgayLapHD;  // Ngày lập hóa đơn
        public char Loai;           // Loại hóa đơn (N: nhập, X: xuất)
        public Ct_Hoadon_List cthd; // Danh sách liên kết đơn các chi tiết hóa đơn
        public Hoadon next;         // Con trỏ next trỏ đến phần tử kế tiếp của danh sách

        public Hoadon(string sohd, DateTime ngaylap, char loai)
        {
            SoHD = sohd;
            NgayLapHD = ngaylap;
            Loai = loai;
            cthd = new Ct_Hoadon_List(); // Khởi tạo danh sách chi tiết hóa đơn
            next = null;
        }



    }



    class Hoadon_List
    {
        public Hoadon head;
        public Hoadon tail;

        public Hoadon_List()
        {
            head = null;
            tail = null;
        }



        public void Add(string sohd, DateTime ngaylap, char loai)
        {
            Hoadon node = new Hoadon(sohd, ngaylap, loai);

            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.next = node;
                tail = node;
            }
        }

        public void Add(Hoadon node)
        {
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.next = node;
                tail = node;
            }
        }




        public void LapHoaDonNhap(DSNhanVien dsNhanVien, QLVT qlvt)
        {
            // Nhập thông tin hóa đơn
            Console.WriteLine("Lập hóa đơn nhập:");
            Console.Write("Nhập số hóa đơn: ");
            string sohd = Console.ReadLine();

            DateTime ngaylap;
            while (true)
            {
                Console.Write("Nhập ngày lập (dd/mm/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaylap))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ngày lập không hợp lệ, vui lòng nhập lại!");
                }
            }


            char loaihd = 'N';
            Console.WriteLine("Loại hóa đơn là: " + loaihd + " (Nhập) ");
            // Tạo hóa đơn mới
            Hoadon hd = new Hoadon(sohd, ngaylap, loaihd);

            // Tạo đối tượng Ct_Hoadon_List
            Ct_Hoadon_List ctHoadonList = new Ct_Hoadon_List(); // Tạo danh sách chi tiết hóa đơn mới


            // Nhập thông tin các chi tiết hóa đơn
            bool hasMavt = false;
            while (true)
            {
                Console.Write("Nhập mã vật tư (nhập END để kết thúc): ");
                string mavt = Console.ReadLine();

                if (mavt.ToUpper() == "END")
                {
                    if (hasMavt)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Phải nhập ít nhất một mã vật tư trước khi kết thúc!");
                        continue;
                    }
                }

                hasMavt = true;

                Vattu vatTu = qlvt.TimVT(mavt);

                if (vatTu == null)
                {
                    Console.WriteLine("Mã VT không tồn tại. Vui lòng nhập lại.");
                    continue;
                }

                Console.Write("Nhập số lượng: ");
                int soluong;
                while (!int.TryParse(Console.ReadLine(), out soluong) || soluong <= 0)
                {
                    Console.WriteLine("Số lượng không hợp lệ, vui lòng nhập lại!");
                }

                Console.Write("Nhập đơn giá: ");
                int dongia;
                while (!int.TryParse(Console.ReadLine(), out dongia) || dongia <= 0)
                {
                    Console.WriteLine("Đơn giá không hợp lệ, vui lòng nhập lại!");
                }

                Console.Write("Nhập %VAT: ");
                int vat;
                while (!int.TryParse(Console.ReadLine(), out vat) || vat < 0)
                {
                    Console.WriteLine("%VAT không hợp lệ, vui lòng nhập lại!");
                }



                // Thêm chi tiết hóa đơn vào danh sách
                ctHoadonList.Add(mavt, soluong, dongia, vat);

                // Cập nhật số lượng tồn của vật tư
                if (char.ToUpper(loaihd) == 'N')
                {
                    qlvt.CapNhatSoLuongTon(mavt, soluong);
                }
                else if (char.ToUpper(loaihd) == 'X')
                {
                    qlvt.CapNhatSoLuongTon(mavt, -soluong);
                }


            }
            // Gán danh sách chi tiết hóa đơn cho hóa đơn



            hd.cthd = ctHoadonList;

            // Tìm nhân viên lập hóa đơn
            Console.Write("Nhập mã nhân viên lập hóa đơn: ");
            string maNV = Console.ReadLine();
            Nhanvien nv = dsNhanVien.TimNhanVien(maNV);

            ////Kiểm tra danh sách chi tiết hóa đơn trống
            //if (ctHoadonList.head == null)
            //{
            //    Console.WriteLine("Danh sách chi tiết hóa đơn trống. Hãy nhập ít nhất một mã vật tư!");

            //}
            //else
            //{
            //    Console.WriteLine("Danh sách chi tiết hóa đơn KHÔNG trống.");
            //}

            if (nv == null)
            {
                Console.WriteLine("Không tìm thấy nhân viên có mã {0}", maNV);
            }
            else
            {
                // Thêm hóa đơn vào danh sách hóa đơn của nhân viên
                nv.DS_HOADON.Add(hd);


                Console.WriteLine("Lập hóa đơn nhập thành công!");
            }
        }


        public void LapHoaDonXuat(DSNhanVien dsNhanVien, QLVT qlvt)
        {
            // Nhập thông tin hóa đơn
            Console.WriteLine("Lập hóa đơn xuất:");
            Console.Write("Nhập số hóa đơn: ");
            string sohd = Console.ReadLine();

            DateTime ngaylap;
            while (true)
            {
                Console.Write("Nhập ngày lập (dd/MM/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaylap))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ngày lập không hợp lệ, vui lòng nhập lại!");
                }
            }

            char loaihd = 'X';
            Console.WriteLine("Loại hóa đơn là: " + loaihd + " (Xuất) ");
            // Tạo hóa đơn mới
            Hoadon hd = new Hoadon(sohd, ngaylap, loaihd);


            // Tạo đối tượng Ct_Hoadon_List
            Ct_Hoadon_List ctHoadonList = new Ct_Hoadon_List();

            // Nhập thông tin các chi tiết hóa đơn
            while (true)
            {
                Console.Write("Nhập mã vật tư (nhập END để kết thúc): ");
                string mavt = Console.ReadLine();
                if (mavt.ToUpper() == "END")
                {
                    break;
                }

                Vattu vatTu = qlvt.TimVT(mavt);

                if (vatTu == null)
                {
                    Console.WriteLine("Ma VT khong ton tai. Vui long nhap lai.");
                    continue;
                }
                Console.Write("Nhập số lượng: ");
                int soluong = int.Parse(Console.ReadLine());

                // Kiểm tra số lượng tồn trong kho
                int soluongTon = qlvt.SoLuongTon(mavt);
                if (soluong > soluongTon)
                {
                    Console.WriteLine("Số lượng xuất vượt quá số lượng tồn trong kho ({0})", soluongTon);
                    continue;
                }

                Console.Write("Nhập đơn giá: ");
                int dongia = int.Parse(Console.ReadLine());

                Console.Write("Nhập %VAT: ");
                int vat = int.Parse(Console.ReadLine());

                // Thêm chi tiết hóa đơn vào danh sách
                ctHoadonList.Add(mavt, soluong, dongia, vat);

                // Gán danh sách chi tiết hóa đơn cho hóa đơn
                hd.cthd = ctHoadonList;

                // Cập nhật số lượng tồn của vật tư
                qlvt.CapNhatSoLuongTon(mavt, -soluong);
            }

            // Tìm nhân viên lập hóa đơn
            Console.Write("Nhập mã nhân viên lập hóa đơn: ");
            string maNV = Console.ReadLine();
            Nhanvien nv = dsNhanVien.TimNhanVien(maNV);

            if (nv == null)
            {
                Console.WriteLine("Không tìm thấy nhân viên có mã {0}", maNV);
            }
            else
            {
                // Thêm hóa đơn vào danh sách hóa
                // Thêm hóa đơn vào danh sách hóa đơn của nhân viên
                // Thêm hóa đơn vào danh sách hóa đơn của nhân viên

                nv.DS_HOADON.Add(hd);

                Console.WriteLine("Lập hóa đơn xuất thành công!");
            }
        }




        public double TinhTongTien(Ct_Hoadon_List dscthdList)
        {
            double tongTien = 0.0;
            Ct_Hoadon current = dscthdList.head;

            while (current != null)
            {
                double thanhTien = current.Soluong * current.Dongia;
                double tienVAT = thanhTien * current.VAT / 100;
                double tongTienCTHD = thanhTien + tienVAT;
                tongTien += tongTienCTHD;

                current = current.next;
            }

            return tongTien;
        }

        public void ThongKeHoaDonTrongKhoangThoiGian(DSNhanVien dsNhanVien)
        {
            Console.WriteLine("THỐNG KÊ CÁC HÓA ĐƠN TRONG KHOẢNG THỜI GIAN");

            // Nhập thời điểm bắt đầu và kết thúc

            DateTime fromDate;
            DateTime toDate;

            while (true)
            {
                Console.Write("Từ ngày (dd/MM/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fromDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ngày không hợp lệ, vui lòng nhập lại!");
                }
            }

            while (true)
            {
                Console.Write("Đến ngày (dd/MM/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out toDate))
                {
                    if (toDate >= fromDate)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ngày kết thúc không được nhỏ hơn ngày bắt đầu. Vui lòng nhập lại!");
                    }
                }
                else
                {
                    Console.WriteLine("Ngày không hợp lệ, vui lòng nhập lại!");
                }
            }


            // In tiêu đề bảng
            Console.WriteLine("Số HĐ\tNgày lập\tLoại HĐ\tHọ tên NV lập\tTrị giá hóa đơn");

            // Duyệt qua danh sách nhân viên
            for (int i = 0; i < dsNhanVien.soLuongNV; i++)
            {
                Nhanvien nv = dsNhanVien.danhSachNV[i];


                // Duyệt qua danh sách hóa đơn của nhân viên
                Hoadon current = nv.DS_HOADON.head;
                while (current != null)
                {
                    // Tạo biến họ tên của nhân viên
                    string hoTen = nv.HO + " " + nv.TEN;
                    // Kiểm tra xem hóa đơn có nằm trong khoảng thời gian hay không
                    if (current.NgayLapHD >= fromDate && current.NgayLapHD <= toDate)
                    {
                        double totalValue = TinhTongTien(current.cthd);
                        // In thông tin hóa đơn
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", current.SoHD, current.NgayLapHD.ToString("dd/MM/yyyy"), current.Loai, hoTen, totalValue);
                    }

                    current = current.next;
                }
            }
        }


        public void InHoaDonTheoSoHoaDon(DSNhanVien dsNhanVien, string soHoaDon)
        {
            if (dsNhanVien == null || dsNhanVien.danhSachNV == null)
            {
                Console.WriteLine("Không có nhân viên hoặc danh sách nhân viên rỗng.");
                return;
            }

            Console.WriteLine("Danh sách hóa đơn có số hóa đơn {0}:", soHoaDon);

            bool foundHoaDon = false;

            foreach (Nhanvien nv in dsNhanVien.danhSachNV)
            {
                if (nv == null || nv.DS_HOADON == null || nv.DS_HOADON.head == null)
                {
                    continue;
                }

                Hoadon_List dsHoaDon = nv.DS_HOADON;

                Hoadon currentHoaDon = dsHoaDon.head;
                while (currentHoaDon != null)
                {
                    if (currentHoaDon.SoHD == soHoaDon)
                    {
                        foundHoaDon = true;
                        Console.WriteLine("Nhân viên: {0} {1}", nv.HO, nv.TEN);
                        Console.WriteLine("Số hóa đơn: {0}", currentHoaDon.SoHD);
                        Console.WriteLine("Ngày lập: {0}", currentHoaDon.NgayLapHD.ToString("dd/MM/yyyy"));
                        Console.WriteLine("Loại hóa đơn: {0}", currentHoaDon.Loai);
                        Console.WriteLine("Chi tiết hóa đơn:");

                        Ct_Hoadon_List ctHoaDonList = currentHoaDon.cthd;

                        if (ctHoaDonList == null || ctHoaDonList.head == null)
                        {
                            Console.WriteLine("Không có chi tiết hóa đơn.");
                        }
                        else
                        {
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine("| Mã VT | Số lượng | Đơn giá |   VAT   |");
                            Console.WriteLine("-----------------------------------------");

                            Ct_Hoadon currentCtHoaDon = ctHoaDonList.head;
                            while (currentCtHoaDon != null)
                            {
                                Console.WriteLine("| {0,-5} | {1,-8} | {2,-7} | {3,-7} |", currentCtHoaDon.MAVT, currentCtHoaDon.Soluong, currentCtHoaDon.Dongia, currentCtHoaDon.VAT);
                                Console.WriteLine("--------------------------------------");

                                currentCtHoaDon = currentCtHoaDon.next;
                            }
                        }

                        Console.WriteLine();
                    }

                    currentHoaDon = currentHoaDon.next;
                }
            }

            if (!foundHoaDon)
            {
                Console.WriteLine("Không tìm thấy hóa đơn có số hóa đơn {0}.", soHoaDon);
            }
        }



    }


    class Ct_Hoadon
    {
        public string MAVT;     // Mã vật tư
        public int Soluong;     // Số lượng
        public double Dongia;    // Đơn giá
        public double VAT;         // %VAT
        public Ct_Hoadon next;  // Con trỏ next trỏ đến phần tử kế tiếp của danh sách


        public Ct_Hoadon(string mavt, int soluong, double dongia, double vat)
        {
            MAVT = mavt;
            Soluong = soluong;
            Dongia = dongia;
            VAT = vat;
            next = null;
        }



    }

    class Ct_Hoadon_List
    {
        public Ct_Hoadon head;
        public Ct_Hoadon tail;

        public Ct_Hoadon_List()
        {
            head = null;
            tail = null;
        }

        public void Add(string mavt, int soluong, double dongia, double vat)
        {
            Ct_Hoadon node = new Ct_Hoadon(mavt, soluong, dongia, vat);

            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.next = node;
                tail = node;
            }
        }


    }



    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            QLVT qlvt = new QLVT();
            DSNhanVien dsNhanVien = new DSNhanVien();
            Hoadon_List hoadonList = new Hoadon_List();
            Ct_Hoadon_List dsChiTietHoaDon = new Ct_Hoadon_List();


            Doc_Ghi_file docGhiFile = new Doc_Ghi_file(); // Tạo thể hiện của lớp Doc_Ghi_file



            // Đọc danh sách vật tư từ file
            docGhiFile.DocFileVattu("vattu.txt", qlvt);

            // Đọc file nhân viên
            string tenFileDoc = "nhanvien.txt";
            docGhiFile.DocFileNhanVien(tenFileDoc, dsNhanVien);


            // Đọc danh sách hóa đơn từ file

            docGhiFile.DocFileHoaDon("hoadon.txt", hoadonList);

            docGhiFile.DocFileChiTietHoaDon("chitiethoadon.txt", dsChiTietHoaDon);

            ////// Kiểm tra và hiển thị thông tin về tệp vattu.txt
            //string currentDirectory = Environment.CurrentDirectory;
            //string filePath = Path.Combine(currentDirectory, "vattu.txt");
            //bool fileExists = File.Exists(filePath);
            //if (fileExists)
            //{
            //    Console.WriteLine("File vattu.txt tồn tại : " + currentDirectory);
            //}
            //else
            //{
            //    Console.WriteLine("File vattu.txt không tồn tại: " + currentDirectory);
            //}


            ////// Kiểm tra và hiển thị thông tin về tệp vattu.txt
            //string currentDirectory1 = Environment.CurrentDirectory;
            //string filePath1 = Path.Combine(currentDirectory, "nhanvien.txt");
            //bool fileExists1 = File.Exists(filePath);
            //if (fileExists)
            //{
            //    Console.WriteLine("File nhanvien.txt tồn tại : " + currentDirectory1);
            //}
            //else
            //{
            //    Console.WriteLine("File nhanvien.txt không tồn tại: " + currentDirectory1);
            //}

            while (true)
            {


                Console.WriteLine(new string('*', 72));
                Console.WriteLine("**" + "MENU".PadLeft(23).PadRight(68) + "**");
                Console.WriteLine(new string('*', 72));

                Console.WriteLine("** " + "1. Them vat tu".PadRight(68) + "**");
                Console.WriteLine("** " + "2. Xóa".PadRight(68) + "**");
                Console.WriteLine("** " + "3. Hieu chinh vat tu".PadRight(68) + "**");
                Console.WriteLine("** " + "4. In danh sach vat tu ton kho theo mã tăng dần".PadRight(68) + "**");
                Console.WriteLine("** " + "5. Nhập nhân viên".PadRight(68) + "**");
                Console.WriteLine("** " + "6. Tìm kiếm nhân viên".PadRight(68) + "**");
                Console.WriteLine("** " + "7. Chỉnh sửa nhân viên ".PadRight(68) + "**");
                Console.WriteLine("** " + "8. In danh sách nhân viên theo thứ tự tên nhân viên tăng dần".PadRight(68) + "**");
                Console.WriteLine("** " + "9. Lap hoa don nhap".PadRight(68) + "**");
                Console.WriteLine("** " + "10. Lap hoa don xuat".PadRight(68) + "**");
                Console.WriteLine("** " + "11. Thống kê các hóa đơn trong khoảng thời gian".PadRight(68) + "**");
                Console.WriteLine("** " + "12. In danh sách vật tư tồn kho theo tên tăng dần".PadRight(68) + "**");
                Console.WriteLine("** " + "13. In hóa đơn dựa vào số hóa đơn".PadRight(68) + "**");

                Console.WriteLine("** " + "0. Thoat".PadRight(68) + "**");

                Console.WriteLine(new string('*', 72));
                Console.Write("Chon chuc nang: ");

                int chucnang = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (chucnang)
                {
                    case 1:
                        qlvt.ThemVT();
                        // Ghi danh sách vật tư vào file


                        break;


                    case 2:
                        Console.Write("Nhap ma vat tu can xoa: ");
                        string mavt1 = Console.ReadLine().ToUpper();
                        qlvt.XoaVatTu(mavt1);



                        break;

                    case 3:

                        Console.Write("Nhap ma vat tu can hieu chinh: ");
                        string mavt = Console.ReadLine().ToUpper();

                        Console.Write("Nhap ten vat tu moi: ");
                        string tenvt = Console.ReadLine();
                        Console.Write("Nhap don vi tinh moi: ");
                        string dvt = Console.ReadLine();
                        Console.Write("Nhap so luong ton moi: ");
                        int slton = int.Parse(Console.ReadLine());

                        qlvt.HieuChinhVatTu(mavt, tenvt, dvt, slton);


                        Vattu vt = qlvt.TimVT(mavt);
                        if (vt != null)
                        {

                            Console.WriteLine("Hieu chinh thanh cong!");
                        }
                        else
                        {
                            Console.WriteLine("Bạn vui lòng kiểm tra lại, hiệu chỉnh không thành công");
                        }

                        break;

                    case 4:
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine(new string('-', 70));
                        Console.WriteLine("|" + " Danh Sách Vật Tư".PadLeft(23).PadRight(68) + "|");
                        Console.WriteLine(new string('-', 70));
                        Console.WriteLine("*" + "MaVT".PadRight(10) + "TenVT".PadRight(30) + "DVT".PadRight(10) + "SoLuongTon".PadRight(15) + "*");
                        // Console.WriteLine(new string('-', 70));
                        qlvt.InDanhSachVatTuTonKho(qlvt.root);
                        Console.WriteLine();
                        
                        Console.WriteLine();
                        Console.WriteLine();


                        break;
                    case 5:
                        Console.Write("Nhap  nhan vien: ");
                        dsNhanVien.NhapNhanVien(dsNhanVien);
                        break;
                    case 6:
                        dsNhanVien.TimKiemNhanVien(dsNhanVien);

                        break;


                    case 7:
                        Console.Write("Nhập mã nhân viên cần chỉnh : ");
                        string mnv = Console.ReadLine().ToUpper();
                        dsNhanVien.ChinhSuaNhanVien(mnv);

                        break;
                    case 8:
                        dsNhanVien.InDanhSachNhanVienTheoThuTuTen(dsNhanVien);
                        break;

                    case 9:
                        hoadonList.LapHoaDonNhap(dsNhanVien, qlvt);
                        break;
                    case 10:
                        hoadonList.LapHoaDonXuat(dsNhanVien, qlvt);
                        break;
                    case 11:

                        hoadonList.ThongKeHoaDonTrongKhoangThoiGian(dsNhanVien);
                        Console.ReadLine();
                        break;
                    case 12:

                        qlvt.InDanhSachVatTuTonKho2();
                        Console.ReadLine();
                        break;
                    case 13:
                        Console.Write("Nhập số hóa đơn cần in: ");
                        string soHoaDon = Console.ReadLine();

                        hoadonList.InHoaDonTheoSoHoaDon(dsNhanVien, soHoaDon);
                        Console.ReadLine();
                        break;


                    case 0:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Chuc nang khong hop le!");
                        break;
                }

                // Ghi danh sách vật tư vào file
                docGhiFile.GhiFileVattu("vattu.txt", qlvt);

                // Ghi file nhân viên
                string tenFileGhi = "nhanvien.txt";
                docGhiFile.GhiFileNhanVien(tenFileGhi, dsNhanVien);

                docGhiFile.GhiFileHoaDon("hoadon.txt", hoadonList);

                docGhiFile.GhiFileChiTietHoaDon("chitiethoadon.txt", dsChiTietHoaDon);

            }




        }

    }
}  
