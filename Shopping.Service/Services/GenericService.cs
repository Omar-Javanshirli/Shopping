using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using Shopping.Core.Repositories;
using Shopping.Core.Services;
using Shopping.Core.UnityOfWork;
using System.Linq.Expressions;

namespace Shopping.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;
        private readonly IMapper mapper;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            this.mapper = mapper;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = this.mapper.Map<TEntity>(entity);

            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommmitAsync();

            var newDto = this.mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);
        }

        public  async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var products = this.mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);

            if (product == null)
                return Response<TDto>.Fail("Id not found", 404);

            return Response<TDto>.Success(this.mapper.Map<TDto>(product), 200);
        }

        public  async Task<Response<NoContent>> RemoveAsync(int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
                return Response<NoContent>.Fail("Id not found", 404);

            _genericRepository.Remove(isExistEntity);

            await _unitOfWork.CommmitAsync();
            return Response<NoContent>.Success(204);
        }

        public async  Task<Response<NoContent>> UpdateAsync(TDto entity, int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
                return Response<NoContent>.Fail("Id not found", 404);

            var updateEntity = this.mapper.Map<TEntity>(entity);

            _genericRepository.Update(updateEntity);

            await _unitOfWork.CommmitAsync();
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);
            return Response<IEnumerable<TDto>>.Success
                (this.mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
        }
    }
}
