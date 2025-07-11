using Api.DTOs;
using Api.Modles;

public interface IcommentRepository
{

    Task<List<CommentDTO>> GetAllAsync();
    Task<CommentDTO> CreateAsync(CreatCommentDTO creatCommentDTO,string userId);
    Task<CommentDTO?> GetByIdAsync(int id);
    Task<CommentDTO> Update(int id, UPdateCommentDTO uPdateCommentDTO);
        Task<bool> DeleteAsync(int id);

}