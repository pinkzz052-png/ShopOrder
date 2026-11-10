# Việc để sau (chỉ làm khi xong Happy Path)

## Kiểm tra dữ liệu đầu vào
- [ ] Thẻ kiểm tra trên DTO (bắt buộc, độ dài, khoảng giá trị)
- [ ] Test Postman với dữ liệu sai (hiện trả 500, là bình thường)

## Logic nghiệp vụ (Service)
- [ ] Danh mục phải tồn tại khi thêm/sửa sản phẩm
- [ ] Kiểm tra đủ tồn kho khi tạo đơn
- [ ] Luồng trạng thái đơn (không đi ngược), hủy đơn thì hoàn tồn kho
- [ ] Số điện thoại khách không trùng

## Xử lý lỗi
- [ ] Các lỗi tự định nghĩa (không tìm thấy, xung đột, dữ liệu sai) đặt trong project Models
- [ ] DAL dịch lỗi SQL (547, 2627, 8152) thành lỗi dễ hiểu
- [ ] Bộ xử lý lỗi toàn cục: lỗi -> mã 400/404/409

## Bảo mật
- [ ] Bảng Users, băm mật khẩu, đăng nhập JWT, phân quyền

## Khác
- [ ] Phân trang và lọc danh sách
- [ ] Kiểm thử đơn vị cho Service
- [ ] Ghi nhật ký (logging)
- [ ] Thử Mapster / Dapper / EF Core (tùy chọn)