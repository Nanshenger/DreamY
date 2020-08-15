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
using io.github.Nanshenger.DreamY.Code;

namespace DreamY.Game
{
    public class Game
    {
        //结构体
        public struct Monster
        {
            public long id;public string name;
            public long hp;public long atk;public long def;public long agi;
            public long exp;public long coin;
            public string bonus1; public int b1pr;
            public string bonus2; public int b2pr;
            public string bonus3; public int b3pr;
        }

        public struct Equipment
        {
            public string kind;public long ID;public string name;
            public long hp;public long mp; public long atk; public long def; public long agi;
            public long crt;public long Break;
        }
        //static表示静态 作用就类似于那种全局变量吧
        public static List<Monster> mon = new List<Monster>();//创建怪物列表
        public static List<Equipment> eqi = new List<Equipment>();//创建装备列表


        public static void LoadMonsters()
        {
            string code = File.ReadAllText(@"C:\Users\Administrator\Desktop\MonsterData.txt"); //读取文件所有内容到code
            Console.WriteLine("code=" + code);

            //文件中的换行是\r\n
            string[] lines = code.Split(new char[] { '\r', '\n' }, StringSplitOptions.None); 
            foreach(string test in lines)Console.WriteLine("lines=" + test);
            Game.Monster monster = new Game.Monster();//声明一个新的怪物实例来加入列表
            foreach (string cmd in lines)//把lines数组里每个元素赋值给cmd然后处理
            {
                Console.WriteLine("cmd=" + cmd);//这里的cmd加入从0开始吧，就是=lines[0]= name
                string[] param = cmd.Split(':'); //根据冒号分割
                foreach (string test in param)
                Console.WriteLine("param=" + test);
                switch (param[0])
                {     
                    case ("id"):    monster = new Game.Monster{id = long.Parse(param[1])}; break;
                    case ("name"):  monster.name = param[1];              break;
                    case ("hp"):    monster.hp = long.Parse(param[1]);    break;
                    case ("atk"):   monster.atk = long.Parse(param[1]);   break;
                    case ("def"):   monster.def = long.Parse(param[1]);   break;
                    case ("agi"):   monster.agi = long.Parse(param[1]);   break;
                    case ("coin"):  monster.coin = long.Parse(param[1]); break;
                    case ("exp"):   monster.exp = long.Parse(param[1]); break;
                    case ("bonus1"):monster.bonus1 = param[1];            break;
                    case ("b1pr"):  monster.b1pr = int.Parse(param[1]);   break;
                    case ("bonus2"):monster.bonus2 = param[1];            break;
                    case ("b2pr"):  monster.b2pr = int.Parse(param[1]);   break;
                    case ("bonus3"):monster.bonus3 = param[1];            break;
                    case ("b3pr"):  monster.b3pr = int.Parse(param[1]);   break;
                    case ("end"):   Console.WriteLine
                            ("怪物ID:" + monster.id + "\n" + "怪物名称：" + monster.name + "\n" +
                             "血量:" + monster.hp + "\n"   + "攻击:" + monster.atk + "\n" +
                             "防御:" + monster.def + "\n"  + "敏捷:" + monster.agi + "\n" +
                             "硬币:" + monster.coin + "\n" + "经验:" + monster.exp + "\n" +
                             "掉落一:" +monster.bonus1+"\n"+ "掉一几率:"+monster.b1pr+"\n"+
                             "掉落二:"+monster.bonus2+"\n" + "掉二几率:"+monster.b2pr+"\n"+
                             "掉落三:"+monster.bonus3+"\n" + "掉三几率:"+monster.b3pr+"\n");
                        //把他加入列表
                        Game.mon.Add(monster);break; 
                }
            }
            Console.WriteLine("本次一共加载了" + Game.mon.Count + "个怪物");
        }


        public static void LoadEquipment()//加载装备
        {
            string code = File.ReadAllText(@"C:\Users\Administrator\Desktop\EquipmentData.txt");//读取装备数据
            Console.WriteLine("code=" + code);

            string[] lines = code.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
            foreach (string test in lines) Console.WriteLine("lines=" + test);
            Game.Equipment equipment = new Game.Equipment();
            foreach (string cmd in lines)
            {
                Console.WriteLine("cmd=" + cmd);
                string[] param = cmd.Split(':');
                foreach (string test in param)
                Console.WriteLine("param=" + test);
                switch (param[0])               
                {
                    case ("kind"): equipment = new Game.Equipment {  kind= param[1] }; break;
                    case ("ID"): equipment.ID = long.Parse(param[1]); break;
                    case ("name"): equipment.name = param[1]; break;
                    case ("hp"): equipment.hp = long.Parse(param[1]); break;
                    case ("mp"): equipment.mp = long.Parse(param[1]); break;
                    case ("atk"): equipment.atk = long.Parse(param[1]); break;
                    case ("def"): equipment.def = long.Parse(param[1]); break;
                    case ("agi"): equipment.agi = long.Parse(param[1]); break;
                    case ("crt"): equipment.crt = long.Parse(param[1]); break;
                    case ("Break"): equipment.Break = long.Parse(param[1]); break;
                    case ("end"):Console.WriteLine
                            ("装备种类:" + equipment.kind + "\n" + "装备ID：" + equipment.ID + "\n"+
                                "装备名称：" + equipment.name + "\n" +
                                "血量:" + equipment.hp + "\n" + "蓝量:" + equipment.mp + "\n" + 
                                "攻击:" + equipment.atk + "\n" +"防御:" + equipment.def + "\n" + 
                                "敏捷:" + equipment.agi + "\n" +"暴击:" + equipment.crt + "\n" + "破甲:" + equipment.Break + "\n"); 
                    Game.eqi.Add(equipment);break;//加入列表
                }
            }
            Console.WriteLine("本次一共加载了" + Game.eqi.Count + "件装备");
        }
    }
}
