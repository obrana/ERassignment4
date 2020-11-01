using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EFassinment4
{
    public class DataService
    {
        public List<Category> GetCategories()
        {
            using (var context = new NorthWindContext())
            {
                var cat = context.Categories.ToList();
                return cat;
            }
        }
    }
}