using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafikaProjekt2
{
    class Petla
    {
        private _3Ddemo demo;
        public bool Running { get; private set; }
        public void Load(_3Ddemo demo)
        {
            this.demo = demo;
        }
        public async void Start()
        {

            Running = true;

            DateTime _previousGameTime = DateTime.Now;

            while (Running)
            {
                TimeSpan GameTime = DateTime.Now - _previousGameTime;
                _previousGameTime = _previousGameTime + GameTime;
                demo.viewOnUpdate(GameTime);
                await Task.Delay(8);
            }
        }


        public void Stop()
        {
            Running = false;
        }
    }

}

