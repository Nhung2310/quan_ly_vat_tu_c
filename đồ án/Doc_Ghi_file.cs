using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DO_AN;

namespace DO_AN
{
    internal class Doc_Ghi_file
    {
        public void GhiFileVattu(string tenFile, QLVT qlvt)
        {
            using (StreamWriter sw = new StreamWriter(tenFile))
            {
                GhiFileVattuNode(sw, qlvt.root);
            }
         //   Console.WriteLine("Ghi file thành công!");
        }

        private void GhiFileVattuNode(StreamWriter sw, Vattu node)
        {
            if (node != null)
            {
                GhiFileVattuNode(sw, node.left);
                sw.WriteLine($"{node.MAVT},{node.TENVT},{node.DVT},{node.SoLuongTon}");
                GhiFileVattuNode(sw, node.right);
            }
        }

        public void DocFileVattu(string tenFile, QLVT qlvt)
        {
            if (File.Exists(tenFile))
            {
                qlvt.root = null; // Xóa cây vật tư hiện tại trước khi đọc từ file

                using (StreamReader sr = new StreamReader(tenFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string maVT = parts[0].Trim();
                            string tenVT = parts[1].Trim();
                            string dvt = parts[2].Trim();
                            int soLuongTon;
                            if (int.TryParse(parts[3].Trim(), out soLuongTon))
                            {
                                Vattu vt = new Vattu(maVT, tenVT, dvt, soLuongTon);
                                qlvt.root = qlvt.ThemVT(qlvt.root, vt);
                            }
                        }
                    }
                }
               // Console.WriteLine("Đọc file thành công!");
            }
            else
            {
               // Console.WriteLine("File không tồn tại!");
            }
        }




        public void GhiFileNhanVien(string tenFile, DSNhanVien dsNhanVien)
        {
            using (StreamWriter sw = new StreamWriter(tenFile))
            {
                // Ghi số lượng nhân viên
                sw.WriteLine(dsNhanVien.soLuongNV);

                // Ghi thông tin từng nhân viên
                for (int i = 0; i < dsNhanVien.soLuongNV; i++)
                {
                    Nhanvien nhanVien = dsNhanVien.danhSachNV[i];
                    sw.WriteLine($"{nhanVien.MANV}, {nhanVien.HO}, {nhanVien.TEN}, {nhanVien.PHAI}");

                    // Ghi danh sách hóa đơn của nhân viên
                    GhiFileHoaDon(sw, nhanVien.DS_HOADON);
                }
            }

           // Console.WriteLine("Ghi file thành công!");
        }



        public void GhiFileHoaDon(string tenFile, Hoadon_List dsHoaDon)
        {
            using (StreamWriter sw = new StreamWriter(tenFile))
            {
                GhiFileHoaDon(sw, dsHoaDon);
            }
           // Console.WriteLine("Ghi file hóa đơn thành công!");
        }

        private void GhiFileHoaDon(StreamWriter sw, Hoadon_List dsHoaDon)
        {
            Hoadon current = dsHoaDon.head;
            while (current != null)
            {
                sw.WriteLine($"{current.SoHD}, {current.NgayLapHD}, {current.Loai}");

                // Ghi danh sách chi tiết hóa đơn
                GhiFileChiTietHoaDon(sw, current.cthd);

                current = current.next;
            }
        }

        public void GhiFileChiTietHoaDon(string tenFile, Ct_Hoadon_List dsChiTietHoaDon)
        {
            using (StreamWriter sw = new StreamWriter(tenFile))
            {
                GhiFileChiTietHoaDon(sw, dsChiTietHoaDon);
            }
           // Console.WriteLine("Ghi file chi tiết hóa đơn thành công!");
        }

        private void GhiFileChiTietHoaDon(StreamWriter sw, Ct_Hoadon_List dsChiTietHoaDon)
        {
            Ct_Hoadon current = dsChiTietHoaDon.head;
            while (current != null)
            {
                sw.WriteLine($"{current.MAVT}, {current.Soluong}, {current.Dongia}, {current.VAT}");


                current = current.next;
            }
        }

        public void DocFileNhanVien(string tenFile, DSNhanVien dsNhanVien)
        {
            if (File.Exists(tenFile))
            {
                using (StreamReader sr = new StreamReader(tenFile))
                {
                    // Đọc số lượng nhân viên
                    string soLuongNVString = sr.ReadLine();
                    if (!string.IsNullOrEmpty(soLuongNVString) && int.TryParse(soLuongNVString, out int soLuongNV))
                    {
                        // Đọc thông tin từng nhân viên
                        for (int i = 0; i < soLuongNV; i++)
                        {
                            string line = sr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                string[] parts = line.Split(',');
                                if (parts.Length == 4)
                                {
                                    string maNV = parts[0].Trim();
                                    string ho = parts[1].Trim();
                                    string ten = parts[2].Trim();
                                    string phai = parts[3].Trim();

                                    Nhanvien nhanVien = new Nhanvien(maNV, ho, ten, phai);

                                    // Đọc danh sách hóa đơn của nhân viên
                                    DocFileHoaDon(sr, nhanVien.DS_HOADON);

                                    // Thêm nhân viên vào danh sách
                                    if (dsNhanVien.danhSachNV == null)
                                    {
                                        dsNhanVien.danhSachNV = new Nhanvien[soLuongNV];
                                    }
                                    dsNhanVien.danhSachNV[i] = nhanVien;
                                    dsNhanVien.soLuongNV++;
                                }
                            }
                        }
                    }
                   
                }

                // Console.WriteLine("Đọc file thành công!");
            }

            else
            {
               // Console.WriteLine("File không tồn tại!");
            }
        }


        public void DocFileHoaDon(string tenFile, Hoadon_List dsHoaDon)
        {
            using (StreamReader sr = new StreamReader(tenFile))
            {
                DocFileHoaDon(sr, dsHoaDon);
            }
        }

        private void DocFileHoaDon(StreamReader sr, Hoadon_List dsHoaDon)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length == 3)
                {
                    string soHD = parts[0].Trim();
                    DateTime ngayLapHD = DateTime.Parse(parts[1].Trim());
                    char loai = char.Parse(parts[2].Trim());

                    Hoadon hoadon = new Hoadon(soHD, ngayLapHD, loai);

                    // Đọc danh sách chi tiết hóa đơn
                    DocFileChiTietHoaDon(sr, hoadon.cthd);

                    // Thêm hóa đơn vào danh sách
                    if (dsHoaDon.head == null)
                    {
                        dsHoaDon.head = hoadon;
                        dsHoaDon.tail = hoadon;
                    }
                    else
                    {
                        dsHoaDon.tail.next = hoadon;
                        dsHoaDon.tail = hoadon;
                    }
                }
            }
        }


      
        private void DocFileChiTietHoaDon(StreamReader sr, Ct_Hoadon_List dsChiTietHoaDon)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length == 4)
                {
                    string maVT = parts[0].Trim();
                    int soLuong;
                    double donGia;
                    double vat;

                    if (int.TryParse(parts[1].Trim(), out soLuong) &&
                        double.TryParse(parts[2].Trim(), out donGia) &&
                        double.TryParse(parts[3].Trim(), out vat))
                    {
                        Ct_Hoadon ctHoadon = new Ct_Hoadon(maVT, soLuong, donGia, vat);

                        // Thêm chi tiết hóa đơn vào danh sách
                        if (dsChiTietHoaDon.head == null)
                        {
                            dsChiTietHoaDon.head = ctHoadon;
                            dsChiTietHoaDon.tail = ctHoadon;
                        }
                        else
                        {
                            dsChiTietHoaDon.tail.next = ctHoadon;
                            dsChiTietHoaDon.tail = ctHoadon;
                        }
                    }
                    
                }
            }
        }

        public void DocFileChiTietHoaDon(string tenFile, Ct_Hoadon_List dsChiTietHoaDon)
        {
            using (StreamReader sr = new StreamReader(tenFile))
            {
                DocFileChiTietHoaDon(sr, dsChiTietHoaDon);
            }
        }


    }
}
