# DEMO Code API với .Net core 2.2
- Clone source về.
- Cài mariadb.
- Mở file appsetting.json sử lại thông số DB theo máy (user, pass, db).
- Mở terminal tại thư mục vừa clone về.
- Trên terminal, chạy lệnh: 
```
dotnet restore
```
- Tạo 1 Migration
```
rm -rf ./Migrations && dotnet ef migrations add Migrations
```
- Dựng lại cấu trúc cho db
```
dotnet ef database update
```
- Chạy project
```
dotnet run
```
- NOTE: restart mỗi khi code thay đổi: 
```
dotnet watch run
```
### GET: 
http://localhost:5003/api/users
###
```
dotnet ef database drop && rm -rf Migrations && dotnet ef migrations add Migrations && dotnet ef database update
```
## Tools:
 - vscode:
    - extentions: git lens, git graph, rest client, C# FixFormat (ctrl + shift + i để format code), ...