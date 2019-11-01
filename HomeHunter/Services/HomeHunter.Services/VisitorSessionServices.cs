using HomeHunter.Data;
using HomeHunter.Domain;
using HomeHunter.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Services
{
    public class VisitorSessionServices : IVisitorSessionServices
    {
        private readonly HomeHunterDbContext context;

        public VisitorSessionServices(HomeHunterDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddSessionInTheDb(string ipAddress, string ai_user)
        {
            if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(ai_user))
            {
                return false;
            }

            if (!this.context.VisitorsSessions.Any(x => x.VisitorId == ai_user))
            {
                var visitorSession = new VisitorSession
                {
                    IpAddress = ipAddress,
                    VisitorId = ai_user,
                };

                this.context.VisitorsSessions.Add(visitorSession);
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<long> UniqueVisitorsCount()
        {
            var visitorsCount = await Task.Run(() => this.context.VisitorsSessions.Count());

            return visitorsCount;
        }
    }
}
