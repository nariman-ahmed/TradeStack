using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.interfaces
{
    public interface iCommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment CommentModel);

        Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto updateDto); 

        Task<Comment?> DeleteAsync(int id);
    }
}