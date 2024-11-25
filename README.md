# VinmecHIS
Dưới đây là hướng dẫn chi tiết từng bước để bạn có thể clone và chạy dự án **VinmecHIS**.

## **1. Clone Repository**
Để bắt đầu, bạn cần tải mã nguồn dự án về máy. Mở terminal hoặc command prompt, sau đó chạy lệnh sau:

```bash
git clone https://github.com/vinhveer/VinmecHIS.git
```

Sau khi hoàn tất, chuyển vào thư mục chứa mã nguồn dự án:

```bash
cd VinmecHIS
```

## **2. Cài đặt Database**

### **2.1 Xóa database cũ**
https://github.com/user-attachments/assets/058c3deb-2139-4a6b-b11f-54e19bff2c03

Nếu bạn đã có phiên bản cũ của cơ sở dữ liệu, hãy xóa nó trước khi thực hiện backup database mới. 

- Mở **SQL Server Management Studio (SSMS)** hoặc công cụ quản lý database bạn đang sử dụng.
- Tìm database cũ liên quan đến dự án này.
- Nhấp chuột phải vào database, chọn **Delete**, đảm bảo không còn tồn tại database cũ trong hệ thống.

### **2.2 Restore database mới**
https://github.com/user-attachments/assets/83b647f8-3d21-4764-908e-6290a6ba4d9a
#### Các bước
1. Mở **SQL Server Management Studio** hoặc công cụ quản lý database.
2. Chuột phải vào **Databases** trong **Object Explorer**, chọn **Restore Database...**.
3. Chọn file backup của database được cung cấp:
   - Đảm bảo bạn đã tải file backup về (ví dụ: `VinmecHIS.bak`).
   - Nhấn **Device**, chọn file `.bak` từ thư mục lưu trữ.

4. Chọn **OK** để thực hiện quá trình restore.
5. Sau khi hoàn tất, kiểm tra database đã được restore thành công trong danh sách **Databases**.
6. Chạy Scripts data_vinmec.sql để insert dữ liệu. Có một số lỗi vì data này chưa sạch sẽ.

## **3. Cấu hình Connection String**

Mỗi project trong solution có một file cấu hình riêng (thường là `web.config`). Bạn cần cập nhật **connection string** để kết nối với database đã cài đặt.

### **Các bước thực hiện:**
1. Mở solution trong **Visual Studio**.
2. Tìm file **web.config** trong thư mục chính của từng project (ví dụ: `Admin`, `Doctors`, `Patients`,...).
3. Tìm dòng cấu hình `connectionStrings` và thay đổi thông tin
   ```xml
   <connectionStrings>
       <add name="DefaultConnection" connectionString="Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
5. Lưu thay đổi trong tất cả các file `web.config`.

---

## **4. Chạy Solution**

### **4.1 Build project**
1. Trong Visual Studio, nhấn **Ctrl + Shift + B** hoặc chọn **Build > Build Solution**.
2. Đảm bảo không có lỗi xảy ra trong quá trình build.

### **4.2 Chạy dự án**
https://github.com/user-attachments/assets/6e17101a-ae3a-4944-a961-3b1f2ea787d8


