using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToDoList.WebApp.Startup))]
namespace ToDoList.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
