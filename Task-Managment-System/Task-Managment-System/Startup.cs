using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Task_Managment_System.Startup))]
namespace Task_Managment_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
