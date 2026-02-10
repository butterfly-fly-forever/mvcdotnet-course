using Microsoft.AspNetCore.Http;
using BookSale.Management.Application.Dtos;

namespace BookSale.Management.Domain.Abstracts
{
    public interface IImageService
    {
        Task<ResponseModel> SaveImageAsync(List<IFormFile> images, string path, string? defaultName);
    }
}