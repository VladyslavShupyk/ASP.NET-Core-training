using System;
using CMSys.Core.Entities.Membership;
using CMSys.Core.Repositories.Membership;
using CMSys.Infrastructure;
using System.Collections.Generic;
using CMSys.Core.Entities.Catalog;
using CMSys.Core.Repositories.Catalog;
using CMSys.Common.Paging;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions { 
                ConnectionString = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=CMSys.Database;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False" 
            };
            UnitOfWork unitOfWork = new UnitOfWork(unitOfWorkOptions);
            IUserRepository userRepository = unitOfWork.UserRepository;
            PageInfo pageInfo = new PageInfo(3, 10);
            PagedList<User> pageUsers = userRepository.GetPagedList(pageInfo,"");
            Console.WriteLine(pageUsers.TotalPages);
            foreach(var user in pageUsers.Items)
            {
                Console.WriteLine(user.Email);
            }
        }
    }
}
