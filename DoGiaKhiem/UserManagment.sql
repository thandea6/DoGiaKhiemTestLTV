CREATE TABLE Users ( -- bảng quản lý thông tin người dùng
    user_id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,  -- mã người dùng
    full_name NVARCHAR(255) NOT NULL, --tên người dùng
    birth_date DATE, -- ngày sinh
    email_address NVARCHAR(255) UNIQUE, -- địa chỉ email
    phone_number NVARCHAR(20), -- số điện thoại
    address NVARCHAR(500), -- địa chỉ
);

