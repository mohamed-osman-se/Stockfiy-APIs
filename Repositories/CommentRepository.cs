using Api.DTOs;
using Api.Modles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : IcommentRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CommentRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CommentDTO> CreateAsync(CreatCommentDTO creatCommentDTO, string userId)
    {
        var comment = _mapper.Map<Comment>(creatCommentDTO);
        comment.AppUserId = userId;
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return _mapper.Map<CommentDTO>(comment);
    }


    public async Task<List<CommentDTO>> GetAllAsync()
    {
        return await _context.Comments
            .AsNoTracking()
            .ProjectTo<CommentDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }



    async Task<CommentDTO?> IcommentRepository.GetByIdAsync(int id)
    {
        return await _context.Comments
            .AsNoTracking()
            .Where(c => c.Id == id)
            .ProjectTo<CommentDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }


    public async Task<CommentDTO> Update(int id, UPdateCommentDTO uPdateCommentDTO)
    {
        var dbComment = await _context.Comments.FindAsync(id);
        if (dbComment == null)
            return null!;
        _mapper.Map(uPdateCommentDTO, dbComment);
        await _context.SaveChangesAsync();
        return _mapper.Map<CommentDTO>(dbComment);

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return false;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
}