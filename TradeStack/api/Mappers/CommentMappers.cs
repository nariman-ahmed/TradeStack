using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        //basic DTO for sending in the request body
        public static CommentDto ToCommentDto(this Comment CommentModel)
        {
            return new CommentDto
            {
                Id = CommentModel.Id,
                Title = CommentModel.Title,
                Content = CommentModel.Content,
                CreatedOn = CommentModel.CreatedOn,
                StockId = CommentModel.StockId
            };
        }

        public static Comment ToCommentFromCreatedDto(this CreateCommentRequestDto CreateDto, int stockid)
        {
            return new Comment
            {
                Title = CreateDto.Title,
                Content = CreateDto.Content,
                StockId = stockid
            };
        }

        public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto UpdateDto)
        {
            return new Comment
            {
                Title = UpdateDto.Title,
                Content = UpdateDto.Content
            };
        }
    }
}