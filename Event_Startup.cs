using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataArrange.Storages;
using System.IO;
using Native.Sdk.Cqp.Model;
using DreamY.Game;
using System.Runtime.InteropServices;

//这样说吧。。。一个namespace是一个MC的mod，他们彼此分离
//你可以让你的代码用这些mod，就是这句话头上的那些using
//然后一个mod里面有很多方块（Class），然后动态方块就是指，你得放置一个方块（创建Class）才能用它的那种（工作台）
//然后静态(static)，就是不用放置也可以用的（地图）
namespace io.github.Nanshenger.DreamY.Code
{

    public class Global //this is a class named Global
    {
        public static CQApi api;
    }

    public class Event_Startup : ICQStartup // while this is another class named Event_Startup in the same namespace
    {//how to understand namespace
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            Global.api = e.CQApi;
            //这是酷Q启动事件
            //Group master = new Group(e.CQApi, 490623220); //根据QQ号和酷QApi对象创建QQ私聊聊天对象
            AllocConsole();
            Console.WriteLine("DreamY启动成功！");

            Game.LoadMonsters();
            Game.LoadEquipment();
        }
    }
}