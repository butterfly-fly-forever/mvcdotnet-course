# Đánh giá Clean Architecture cho dự án BookSale.Management

Dựa trên phân tích cấu trúc dự án và mã nguồn, dưới đây là các vấn đề chính và vi phạm nguyên tắc Clean Architecture được tìm thấy:

## 1. Vi phạm quy tắc phụ thuộc (Dependency Rule) - Quan trọng nhất

Clean Architecture yêu cầu các lớp bên trong (Domain, Application) **không được phụ thuộc** vào các lớp bên ngoài (Infrastructure, DataAccess, UI). Tuy nhiên, dự án hiện tại có các vi phạm nghiêm trọng:

*   **Application phụ thuộc DataAccess:**
    *   Trong `BookSale.Management.Application.csproj`, có tham chiếu trực tiếp đến `BookSale.Management.DataAccess`.
    *   Điều này cho phép tầng Application sử dụng các lớp cụ thể của DataAccess, phá vỡ tính trừu tượng và làm khó khăn cho việc thay thế database hoặc unit test.
*   **Application phụ thuộc UI:**
    *   Trong `BookService.cs`, có `using BookSale.Management.UI.Models;`.
    *   Tầng Application không bao giờ được biết về tầng UI. Các Model trả về (như `ResponseDatatable`, `ResponseModel`) nên là DTO (Data Transfer Object) nằm trong tầng Application hoặc Shared Kernel, không phải ở UI.
*   **Domain phụ thuộc Framework:**
    *   Trong `BookSale.Management.Domain.csproj`, có tham chiếu đến `Microsoft.EntityFrameworkCore` và `Dapper`.
    *   Domain Entities (`Book.cs`) sử dụng các Attributes của EF Core (`[Key]`, `[ForeignKey]`, `[Table]`). Điều này làm Domain bị dính chặt vào Database Framework. Domain nên là POCO (Plain Old CLR Objects) thuần túy.

## 2. Rò rỉ chi tiết hạ tầng vào Domain (Leaky Abstraction)

*   **ISQLQueryHandler trong Domain:**
    *   Interface `ISQLQueryHandler` được định nghĩa trong Domain nhưng chứa các phương thức mang tính kỹ thuật hạ tầng như `ExecuteStoreProdecureReturnListAsync` và sử dụng `DynamicParameters` của Dapper.
    *   **Tại sao tệ?** Domain nói chuyện bằng ngôn ngữ nghiệp vụ (ví dụ: `FindBooksByAuthor`), không nên nói chuyện bằng ngôn ngữ SQL/Database (ví dụ: `ExecuteStoredProcedure`). Nếu sau này bạn chuyển sang NoSQL hoặc WebAPI khác, interface này sẽ vô nghĩa.

## 3. Interface đặt sai vị trí

*   **IPDFService:**
    *   Interface này nằm trong `BookSale.Management.Infrastruture`.
    *   Nếu tầng Application muốn sử dụng tính năng xuất PDF, nó buộc phải tham chiếu đến Infrastructure (vi phạm quy tắc phụ thuộc).
    *   **Giải pháp:** Di chuyển `IPDFService` vào tầng Application (hoặc Domain), và để tầng Infrastructure thực thi (implement) nó.

## 4. Vấn đề đặt tên và cấu trúc khác

*   **Lỗi chính tả:** Tên project là `BookSale.Management.Infrastruture` (thiếu chữ 'r' trong 'Infrastructure').
*   **Infrastructure làm quá nhiều việc:**
    *   `ConfigurationService` trong Infrastructure đang thực hiện việc đăng ký Dependency Injection cho toàn bộ hệ thống (bao gồm cả Application và DataAccess). Mặc dù tiện lợi, điều này tạo ra sự phụ thuộc lẫn nhau cao giữa Infrastructure và các tầng khác.

## Tóm tắt & Khuyến nghị

| Vấn đề | Mức độ | Giải pháp |
| :--- | :--- | :--- |
| Application phụ thuộc DataAccess | **Cao** | Xóa tham chiếu. Sử dụng Interface (Repository Pattern) định nghĩa tại Domain/Application. |
| Application phụ thuộc UI | **Cao** | Di chuyển các Model/DTO xuống tầng Application. |
| Domain Entities dính EF Core | Trung bình | Xóa Attributes, sử dụng Fluent API trong `DbContext` để cấu hình mapping. |
| ISQLQueryHandler trong Domain | Trung bình | Loại bỏ hoặc chuyển xuống DataAccess. Domain chỉ nên chứa Repository Interface thuần. |
| Interface IPDFService sai chỗ | Trung bình | Chuyển Interface về Application, Implementation giữ ở Infrastructure. |
| Tên project sai chính tả | Thấp | Đổi tên `Infrastruture` thành `Infrastructure`. |

Dự án hiện tại đang theo hướng "Pragmatic Architecture" (thực dụng) hơn là Clean Architecture chuẩn mực. Nó có thể chạy tốt cho ứng dụng nhỏ, nhưng sẽ gặp khó khăn khi mở rộng, bảo trì hoặc viết Unit Test độc lập.
