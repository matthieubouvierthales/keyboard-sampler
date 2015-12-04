using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sampler
{
    public class SoundController : ApiController
    {

        public static event EventHandler<int> SoundRequested; 

        // GET
        [HttpGet]
        [Route("{id}")]
        public void Get(int id)
        {
            OnSoundRequested(id);
        }

        // POST
        [HttpPost]
        [Route("{id}")]
        public void Post(int id)
        {
            OnSoundRequested(id);
        }

        private void OnSoundRequested(int soundId)
        {
            var handler = SoundRequested;
            if (handler != null)
                handler(this, soundId);
        }
    }

}
