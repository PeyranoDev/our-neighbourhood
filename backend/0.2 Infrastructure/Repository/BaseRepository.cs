using Domain;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly AqualinaAPIContext _context;

        public BaseRepository(AqualinaAPIContext context)
        {
            this._context = context;
        }


    }
}
