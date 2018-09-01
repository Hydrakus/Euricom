using Euricom.Cruise2018.Demo.Query.PapierSettingPersoon;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Euricom.Cruise2018.Demo.Services.Api.Controllers
{
    [RoutePrefix("api/papiersettingpersoon")]
    public class PapierSettingPersoonController : ApiController
    {
        private readonly IPapierSettingPersoonRepository _repository;

        public PapierSettingPersoonController(IPapierSettingPersoonRepository repository)
        {
            _repository = repository;
        }

        [Route("{id}")]
        public IHttpActionResult GetById(string id)
        {                                         
            return Ok(_repository.GetQueryable().SingleOrDefault(x => x.PapierSettingPersoonId == id));
        }
    }
} 