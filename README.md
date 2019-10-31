# Đồ án môn học Công Nghệ Phần Mềm
## Yêu cầu tối thiểu:
- Đã cài dotnet core 3.0
- Đã cài nodejs (để chạy các command bên dưới)
#### Khởi Chạy:
```
npm start
```
#### Tạo Migrations + Xoá DB + Update db
```
npm run resetdb
```
### Các bước khi làm 1 tính năng mới:
- git checkout clean-arch  (nhánh chính)
- git checkout -b new_branch  (tạo 1 nhánh mới)
- => Thực hiện việc code + commit
- git checkout clean-arch  (quay lại nhánh chính)
- git pull  (lấy code mới nhất từ nhánh chính)
- git checkout new_branch  (quay lại nhánh đã tạo)
- git rebase clean-arch  (cập nhật code mới nhất từ nhánh chính về nhánh của mình)
- git push -u origin new_branch (đẩy nhánh lên github)
- => Lên github tạo Pull Request
- => Xong, sau khi tui merge, nhánh new_branch sẽ bị xoá, nên checkout về nhánh clean-arch để làm tiếp.