using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataArrange.Storages;
using Native.Sdk.Cqp;
using System.IO; //引用404的命名空间
using System.Runtime.InteropServices;
using DreamY.Game;
using Native.Sdk.Cqp.Model;
using System.Security.Cryptography.X509Certificates;

namespace io.github.Nanshenger.DreamY.Code
{
    // 添加引用 IGroupMessage
    public class Event_GroupMessage: IGroupMessage
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        public struct Inventory//声明物品栏类型
        {
            public long id;
            public long level;
        }
        public static List<Inventory> inv = new List<Inventory>();
        public void Inventoryload(long QQ)//加载物品栏函数
        {
            Event_GroupMessage.Inventory inventory = new Event_GroupMessage.Inventory();//声明一个新实例来储存临时值
            if (save.getkey(QQ.ToString(), "Inventory", "") == null) save.putkey(QQ.ToString(), "Inventory", ""); //以免出现bug
            if (save.getkey(QQ.ToString(), "Inventory", "") != "") 
            { 
                string[] temp = save.getkey(QQ.ToString(), "Inventory", "").Split(',');
                if (temp.Length != 1) 
                {
                    foreach (string item in temp)
                    {
                        if (item != "")
                        {
                            inventory.id = Convert.ToInt64(item.Substring(0, item.Length - 2));//取出装备ID
                            inventory.level = Convert.ToInt64(item.Substring(item.Length - 2, 2));//取出装备强化等级
                            inv.Add(inventory);//把读取的装备加入列表
                        }
                    }
                }
            }//inv列表已经加载完毕
        }

        public void AddEquipment(long accout, long equipmentID)//向玩家背包添加装备
        {
            string temp = save.getkey(accout.ToString(), "Inventory", "");
            temp = temp + equipmentID.ToString() + "00,";//格式为装备Id+强化等级+,
            save.putkey(accout.ToString(), "Inventory", temp);
            Inventoryload(accout);
            inv.Clear();//清理列表
        }

        public void DeleteEquipment(long accout, long itemnumber)//删除玩家inv列表中的某项装备
        {

            Inventoryload(accout);//加载物品栏
            List<Inventory> temp = new List<Inventory>();//新建一个临时储存的实例
            Int32 tempi = 0;//循环变量
            while (tempi < inv.Count)
            {
                if(itemnumber != tempi) { temp.Add(inv[tempi]);}//检测是否遇到被删除的装备
                tempi = tempi + 1;//下一件装备
            }
            string str = "";
            foreach (Inventory temp2 in temp)
            {
                if (temp2.level > 9) { str = str + temp2.id.ToString() + temp2.level.ToString()+ ","; } 
                else { str = str + temp2.id.ToString() + "0" + temp2.level.ToString() + ","; }//level低于十级添加一个0
            }
            save.putkey(accout.ToString(), "Inventory", str);
            inv.Clear();temp.Clear();//清理列表
        }



        public struct Player
        {
            public int level;//等级
            public int talent;//天赋
            public int energy;//活力值
            public long WATK;public long WDEF;public long Wbreak;public long WAGI;public long WHP;public long WMP;public long WCRT;//攻防破敏血魔暴
            public long AATK; public long ADEF; public long Abreak; public long AAGI; public long AHP; public long AMP; public long ACRT;
            public long OATK; public long ODEF; public long Obreak; public long OAGI; public long OHP; public long OMP; public long OCRT;
            public long HP;public long MP;public long ATK;public long DEF;public long AGI;public long CRT;public long Break;
            public int needEXP;public int nowEXP;
            public string lastact;//最后行动时间
            public int Alkaid;//瑶光祝福等级
            public int VIPrank;//VIP等级
            //此乃构造函数
            public Player(long QQ)
            {
                level = Convert.ToInt32(save.getkey(QQ.ToString(), "level","0"));//获取等级
                talent = Convert.ToInt32(save.getkey(QQ.ToString(), "talent", "0"));//获取资质
                nowEXP = Convert.ToInt32(save.getkey(QQ.ToString(), "nowEXP", "0"));//获取当前拥有经验量
                energy = Convert.ToInt32(save.getkey(QQ.ToString(), "energy", "0"));//获取体力值
                lastact = save.getkey(QQ.ToString(), "lastact", "2020/1/1 11:11:11");//获取上次行动时间
                Alkaid = Convert.ToInt32(save.getkey(QQ.ToString(), "Alkaid", "0"));//获取瑶光祝福等级
                VIPrank = Convert.ToInt32(save.getkey(QQ.ToString(),"VIPrank","0"));//获取VIP等级

                string weaponstr = (save.getkey(QQ.ToString(), "weapon", "-1"));//获取穿着装备的ID+强化等级
                string armorstr = (save.getkey(QQ.ToString(), "armor", "-1"));
                string ornamentstr = (save.getkey(QQ.ToString(), "ornament", "-1"));

                Int32 weaponid = 0;Int32 weaponlevel = 0;
                Int32 armorid = 0; Int32 armorlevel = 0;
                Int32 ornamentid = 0; Int32 ornamentlevel = 0;

                if (weaponstr!="-1")weaponid =Convert.ToInt32( weaponstr.ToString().Substring(0, weaponstr.ToString().Length - 2));//获取ID
                if (armorstr != "-1") armorid = Convert.ToInt32(armorstr.ToString().Substring(0, armorstr.ToString().Length - 2));//获取ID
                if (ornamentstr != "-1") ornamentid = Convert.ToInt32(ornamentstr.ToString().Substring(0, ornamentstr.ToString().Length - 2));//获取ID

                if(weaponstr != "-1")weaponlevel = Convert.ToInt32(weaponstr.Substring(weaponstr.Length-2,2));//获取level
                if (armorstr != "-1") armorlevel = Convert.ToInt32(armorstr.Substring(armorstr.Length-2,2));//获取level
                if (ornamentstr != "-1") ornamentlevel = Convert.ToInt32(ornamentstr.Substring(ornamentstr.Length-2,2));//获取level

                WATK = 0;  WDEF=0;  Wbreak=0;  WAGI=0;  WHP=0;  WMP=0;  WCRT=0;//攻防破敏血魔暴全部初始化
                 AATK=0;  ADEF=0;  Abreak=0;  AAGI=0;  AHP=0;  AMP=0;  ACRT=0;
                 OATK=0;  ODEF=0;  Obreak=0;  OAGI=0;  OHP=0;  OMP=0;  OCRT=0;
                 HP=0;  MP=0;  ATK=0;  DEF=0;  AGI=0;  CRT=0;  Break=0;

                //读取装备的属性
                if (weaponstr != "-1")
                {
                    WATK = Convert.ToInt64( Game.eqi[weaponid].atk*Math.Pow(1.15,weaponlevel));
                    WDEF = Convert.ToInt64(Game.eqi[weaponid].def * Math.Pow(1.15, weaponlevel));
                    Wbreak = Convert.ToInt64(Game.eqi[weaponid].Break * Math.Pow(1.15, weaponlevel));
                    WAGI = Convert.ToInt64(Game.eqi[weaponid].agi * Math.Pow(1.15, weaponlevel));
                    WHP = Convert.ToInt64(Game.eqi[weaponid].hp * Math.Pow(1.15, weaponlevel)); 
                    WMP = Convert.ToInt64(Game.eqi[weaponid].mp * Math.Pow(1.15, weaponlevel)); 
                    WCRT = Convert.ToInt64(Game.eqi[weaponid].crt * Math.Pow(1.15, weaponlevel));
                }
                if (armorstr != "-1")
                {
                    AATK = Convert.ToInt64(Game.eqi[armorid].atk * Math.Pow(1.15, armorlevel));
                    ADEF = Convert.ToInt64(Game.eqi[armorid].def * Math.Pow(1.15, armorlevel));
                    Abreak = Convert.ToInt64(Game.eqi[armorid].Break * Math.Pow(1.15, armorlevel));
                    AAGI = Convert.ToInt64(Game.eqi[armorid].agi * Math.Pow(1.15, armorlevel));
                    AHP = Convert.ToInt64(Game.eqi[armorid].hp * Math.Pow(1.15, armorlevel));
                    AMP = Convert.ToInt64(Game.eqi[armorid].mp * Math.Pow(1.15, armorlevel)); 
                    ACRT = Convert.ToInt64(Game.eqi[armorid].crt * Math.Pow(1.15, armorlevel));
                }
                if (ornamentstr != "-1")
                {
                    OATK = Convert.ToInt64(Game.eqi[ornamentid].atk * Math.Pow(1.15, ornamentlevel));
                    ODEF = Convert.ToInt64(Game.eqi[ornamentid].def * Math.Pow(1.15, ornamentlevel));
                    Obreak = Convert.ToInt64(Game.eqi[ornamentid].Break * Math.Pow(1.15, ornamentlevel));
                    OAGI = Convert.ToInt64(Game.eqi[ornamentid].agi * Math.Pow(1.15, ornamentlevel));
                    OHP = Convert.ToInt64(Game.eqi[ornamentid].hp * Math.Pow(1.15, ornamentlevel));
                    OMP = Convert.ToInt64(Game.eqi[ornamentid].mp * Math.Pow(1.15, ornamentlevel));
                    OCRT = Convert.ToInt64(Game.eqi[ornamentid].crt * Math.Pow(1.15, ornamentlevel));
                }

                needEXP = Convert.ToInt32(Math.Round(4f + level * 10f + 6f * Convert.ToInt32(Math.Pow(1.18f, level)), 0));//计算共需经验
                HP = 100 + (level - 1) * 5 * talent / 100 + WHP + AHP + OHP + Alkaid*3 ;                                  //生命值
                MP = 30 + (level - 1) * 2 * talent / 100 + WMP + AMP + OMP + Alkaid*2 ;                                   //魔法值
                ATK = 20 + (level - 1) * 1 * talent / 100 + WATK + AATK + OATK +Alkaid;                                   //攻击值
                DEF = 10 + ((level - 1)/2) * talent / 100 + WDEF + ADEF + ODEF+ Convert.ToInt32(Alkaid*0.5);              //防御值
                AGI = 10 + WAGI + AAGI + OAGI + Convert.ToInt32(Alkaid*0.2);                                              //敏捷值
                CRT = 0 + WCRT + ACRT + OCRT;                                                                             //暴击率
                Break = 0 + Wbreak + Abreak + Obreak;                                                                     //破甲值
            }
        }                                     //结构体:玩家并且加载数据

        public static Storage save = new Storage("dreamy");         //声明一个404的Storage对象并实例化

        public Boolean rnd(int max, int utmost)                     //返回布尔值
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            if (rd.Next(1, max+1) <= utmost) { return true; } else { return false; }  
        }

        public int numrnd(int max)                                  //返回随机数值
        { 
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            return rd.Next(1,max+1);
        }

        protected bool IsNumberic(string message)                   //检测该字符串是否为数字
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            if (rex.IsMatch(message))
            {
                return true;
            }
            else
                return false;
        }

        public void addcoin(long account, long changecoin)          //金币操作
        {
            long coin = Convert.ToInt32(save.getkey(account.ToString(), "coin", "0"));
            coin = coin + changecoin;
            save.putkey(account.ToString(), "coin", coin.ToString());
        }

        public void addexp(long account,long  changeexp)            //经验操作
        {
            Player p = new Player(account);//加载玩家数据
            p.nowEXP = p.nowEXP + Convert.ToInt32 (changeexp);
            while(p.nowEXP>=p.needEXP)
            {
                p.nowEXP = p.nowEXP - p.needEXP;
                p.level++;//等级提高
                p.energy = p.energy + 10;//升级当前体力增加10
                save.putkey(account.ToString(), "nowEXP", p.nowEXP.ToString());
                save.putkey(account.ToString(),"level",p.level.ToString());
                save.putkey(account.ToString(),"energy",p.energy.ToString());
            }
            save.putkey(account.ToString(), "nowEXP", p.nowEXP.ToString());
            save.putkey(account.ToString(), "level", p.level.ToString());
        }

        public void fight(long QQ, CQGroupMessageEventArgs e)       //pvp模块
        {
            if (QQ.ToString() != save.getkey(QQ.ToString(), "account", ""))
            {
                e.FromGroup.SendGroupMessage("对方未注册帐号！");
            }
            else
            {
            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ),"向",CQApi.CQCode_At(QQ),"发起了战斗！");
            Player p = new Player(QQ);
            Game.Monster enemy = new Game.Monster();
            enemy.atk = p.ATK;
            enemy.agi = p.AGI;
            enemy.coin = 0;
            enemy.def = p.DEF;
            enemy.hp = p.HP;
            enemy.exp = 0;
            enemy.id = QQ;
            enemy.name = e.FromGroup.GetGroupMemberInfo(QQ).Card;
            if(enemy.name == "") enemy.name = e.FromGroup.GetGroupMemberInfo(QQ).Nick;
            enemy.name = "[玩家]'" + enemy.name + "'";
            fight(enemy, e);
            }
        }

        public void fight(int monsterID, CQGroupMessageEventArgs e) //副本模块
        {
            fight(Game.mon[monsterID], e);
        }

        public void fight(Game.Monster Enemy, CQGroupMessageEventArgs e) //战斗模块
        {

            string fighttext="";string temptext; long Round = 0;
            Game.Monster enemy = Enemy;
            long monsterhp = enemy.hp;
            long monsteratk = enemy.atk;
            long monsterdef = enemy.def;
            long monsteragi = enemy.agi;
            Player p = new Player(e.FromQQ.Id);//加载玩家数据
            if ((DateTime.Now-Convert.ToDateTime(p.lastact)).TotalSeconds >= 6*(101-p.level) && p.energy>=10) //CD且体力值允许战斗
            {
                while (monsterhp > 0 && p.HP > 0)
                {
                    if (p.AGI >= monsteragi) //先手
                    {
                        Round++;
                        fighttext = fighttext +"R"+ Round.ToString()+"  ";//记录回合

                        monsterhp = monsterhp - p.ATK*p.ATK/(p.ATK+monsterdef);//攻击怪物
                        if (monsterhp < 0) monsterhp = 0;
                        temptext = "己方伤害:" + (p.ATK * p.ATK / (p.ATK + monsterdef)).ToString()+",余:"+monsterhp.ToString();//小回合记录生成
                        fighttext = fighttext + temptext;//小回合记录
                        if (monsterhp <= 0) break;

                        p.HP = p.HP - Convert.ToInt32 (monsteratk * monsteratk / (monsteratk + p.DEF));//被怪物攻击
                        if (p.HP < 0) p.HP = 0; 
                        temptext = " 敌方伤害:" + Convert.ToInt32(monsteratk * monsteratk/(monsteratk + p.DEF)).ToString() + ",余:" + p.HP.ToString()+"\n";//小回合记录生成
                        fighttext = fighttext + temptext;//小回合记录
                        if (p.HP <= 0) break;

                    }
                    if(p.AGI<=monsteragi) //后手
                    {
                        Round++;
                        fighttext = fighttext + "R" + Round.ToString()+"  ";//记录回合

                        p.HP = p.HP - Convert.ToInt32(monsteratk * monsteratk / (monsteratk + p.DEF));//被怪物攻击
                        if (p.HP < 0) p.HP = 0;
                        temptext = "敌方伤害:" + Convert.ToInt32(monsteratk * monsteratk / (monsteratk + p.DEF)).ToString() + ",余:" + p.HP.ToString();//小回合记录生成
                        fighttext = fighttext + temptext;//小回合记录
                        if (p.HP <= 0) break;

                        monsterhp = monsterhp - p.ATK * p.ATK / (p.ATK + monsterdef);//攻击怪物
                        if (monsterhp < 0) monsterhp = 0;
                        temptext = " 己方伤害:" + (p.ATK * p.ATK / (p.ATK + monsterdef)).ToString() + ",余:" + monsterhp.ToString()+"\n";//小回合记录生成
                        fighttext = fighttext + temptext;//小回合记录
                    }
                }
                if (monsterhp <=0) //战胜怪物
                {
                    e.FromGroup.SendGroupMessage(fighttext+"\n你击败了"+enemy.name+"并获得了硬币:"+enemy.coin.ToString()+",经验:"+enemy.exp.ToString());
                    addcoin(e.FromQQ.Id, enemy.coin);//增加金币
                    addexp(e.FromQQ.Id, enemy.exp);//增加经验

                    if (rnd(100, enemy.b1pr))//获取装备
                    {
                        AddEquipment(e.FromQQ.Id, Convert.ToInt64(enemy.bonus1));//加入背包
                        e.FromGroup.SendGroupMessage("你获得了:" + Game.eqi[Convert.ToInt32(enemy.bonus1)].name);//发送消息
                    }
                    if (rnd(100, enemy.b2pr))
                    {
                        AddEquipment(e.FromQQ.Id, Convert.ToInt64(enemy.bonus2));
                        e.FromGroup.SendGroupMessage("你获得了:" + Game.eqi[Convert.ToInt32(enemy.bonus2)].name);
                    }
                    if (rnd(100, enemy.b3pr))
                    {
                        AddEquipment(e.FromQQ.Id, Convert.ToInt64(enemy.bonus3));
                        e.FromGroup.SendGroupMessage("你获得了:" + Game.eqi[Convert.ToInt32(enemy.bonus3)].name);
                    }
                }
                if (p.HP <= 0) //战败怪物
                {
                    e.FromGroup.SendGroupMessage(fighttext + "\n你被" + enemy.name + "击败了！"
                    + "并失去了硬币:" + (enemy.coin / 2 + p.level*2).ToString()); 
                    addcoin(e.FromQQ.Id, -enemy.coin/2-p.level*2);//扣除金币
                    p.energy = p.energy + 5;//恢复5点
                }
                save.putkey(e.FromQQ.Id.ToString(), "lastact", DateTime.Now.ToString());//存储行动时间
                p.energy = p.energy - 10;
                save.putkey(e.FromQQ.Id.ToString(), "energy", p.energy.ToString());//存储活力值
            }
            else 
            { 
                if (p.energy < 10)
                {
                    e.FromGroup.SendGroupMessage("现在还不能行动！您的活力不足，请明天再来！");
                }
                if ((DateTime.Now - Convert.ToDateTime(p.lastact)).TotalSeconds < 6*(101-p.level))
                {
                    TimeSpan CD = (new TimeSpan(0, 6 * (101 - p.level)/60, 6 * (101 - p.level)% 60)) - (DateTime.Now - Convert.ToDateTime(p.lastact));
                    e.FromGroup.SendGroupMessage("现在还不能行动！挑战冷却未结束（" +
                                                 CD.Minutes.ToString("00") + ":" + CD.Seconds.ToString("00") + "）");
                }
            }
        }



        // 接收事件
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            //错误陷阱
            try
            {
                //开关监测窗口----------------------------------------------------------------------------------------------
                if (e.Message.Text == "dy show" && e.FromQQ.Id == 2584036741) { AllocConsole(); e.FromGroup.SendGroupMessage("The console has been shown."); }
                if (e.Message.Text == "dy hide" && e.FromQQ.Id == 2584036741) { FreeConsole(); e.FromGroup.SendGroupMessage("The console has been hidden."); }

                //进行注册操作----------------------------------------------------------------------------------------------
                if (e.Message.Text == "dy 注册" && e.FromQQ.Id.ToString() != save.getkey(e.FromQQ.Id.ToString(), "account", ""))//注册模块
                {
                    string account = e.FromQQ.Id.ToString();
                    e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "注册成功！");
                    save.putkey(e.FromQQ.Id.ToString(), "account", account);
                    //获取初始天赋等级
                    int talent = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "talent", "0"));//资质检测
                    if (talent == 0) { talent = 89 + numrnd(21); }
                    save.putkey(e.FromQQ.Id.ToString(), "talent", talent.ToString());
                    int level = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "level", "0"));//等级检测
                    if (level == 0) { level = 1; }
                    save.putkey(e.FromQQ.Id.ToString(), "level", level.ToString());
                }

                //以下操作只有注册者才可以生效
                if (e.FromQQ.Id.ToString() == save.getkey(e.FromQQ.Id.ToString(), "account", ""))
                {
                    //签到-------------------------------------------------------------------------------------------------
                    string signintime = save.getkey(e.FromQQ.Id.ToString(), "signintime", "");//读取用户上一次签到时间
                    if (e.Message.Text == "dy 签到")//如果上次签到时间与系统时间不一致
                    {
                        if(signintime != DateTime.Now.ToString("yyyy-MM-dd").ToString())
                        {
                            save.putkey(e.FromQQ.Id.ToString(), "signintime", DateTime.Now.ToString("yyyy-MM-dd").ToString());//存储当前签到时间

                            int SITtime = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "SITtime", "0"));//读取用户签到次数
                            SITtime++;//签到次数+1
                            save.putkey(e.FromQQ.Id.ToString(), "SITtime", SITtime.ToString());//储存当前签到次数

                            int coin = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));//读取用户硬币个数
                            int coinget = 1000 + numrnd(SITtime * 20) ;//随机化签到获取的硬币
                            coin = coin + coinget;//签到硬币+
                            save.putkey(e.FromQQ.Id.ToString(), "coin", coin.ToString());//储存当前硬币个数

                            Player p = new Player(e.FromQQ.Id);//加载玩家数据
                            p.energy = p.level*2 + Convert.ToInt32(p.Alkaid*0.5)+p.VIPrank*2+50;//恢复体力
                            save.putkey(e.FromQQ.Id.ToString(), "energy", p.energy.ToString());//存储体力

                            string temp = "";//临时字符串
                            if (p.VIPrank !=0) {temp = "(V" + p.VIPrank.ToString() + "额外+" + (p.VIPrank * 2).ToString()+"体力)"; }

                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "签到成功，当前签到次数：" + SITtime.ToString()
                                + "，获得硬币：" + coinget.ToString() + "，当前硬币个数：" + coin.ToString()+ "，体力："+p.energy.ToString()
                                +temp);
                        }
                        else
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "你今天已经签到过了，你忘了吗？");
                        }
                    }

                    //查看硬币操作------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy 查看硬币")
                    {
                        int coin = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));
                        e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "当前硬币：" + coin.ToString());
                    }

                    //查看属性操作------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy 查看属性")
                    {
                        int talent = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "talent", "0"));//资质检测
                        if (talent == 0) { talent = 89 + numrnd(21); }
                        save.putkey(e.FromQQ.Id.ToString(), "talent", talent.ToString());
                        int level = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "level", "0"));//等级检测
                        if (level == 0) { level = 1; }
                        save.putkey(e.FromQQ.Id.ToString(), "level", level.ToString());

                        //加载玩家属性
                        Player p = new Player(e.FromQQ.Id);
                        save.getkey(e.FromQQ.Id.ToString(), "nowEXP", "0");//读取当前经验值
                        string temp = "";//临时字符串
                        if (p.VIPrank != 0) { temp = "尊贵的V" + p.VIPrank.ToString(); } 
                        else { save.putkey(e.FromQQ.Id.ToString(), "VIPrank", "0"); }//如果有VIP等级就显示V，没有就储存0
                        e.FromGroup.SendGroupMessage(temp+"账号：" + e.FromQQ.Id.ToString()
                        + "\n等级：" + level.ToString() +" 瑶光等级："+p.Alkaid.ToString()
                        + "\n当前经验：" + p.nowEXP.ToString() 
                        + "\n共需经验：" + p.needEXP.ToString()
                        + "\n生命值：" + p.HP.ToString() 
                        + "\n魔法值：" + p.MP.ToString()
                        + "\n攻击值：" + p.ATK.ToString() 
                        + "\n防御值：" + p.DEF.ToString() 
                        + "\n敏捷值：" + p.AGI.ToString() 
                        + "\n暴击率："+ p.CRT.ToString()
                        + "\n破甲值：" + p.Break.ToString() 
                        + "\n天赋值：" + talent.ToString() 
                        + "\n体力值：" + p.energy.ToString());
                    }

                    //查看装备操作------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy 查看装备")
                    {
                        string weapon = save.getkey(e.FromQQ.Id.ToString(), "weapon", "");
                        string armor = save.getkey(e.FromQQ.Id.ToString(), "armor", "");
                        string ornament = save.getkey(e.FromQQ.Id.ToString(), "ornament", "");
                        string temp = "";//要发送的消息
                        if (weapon != "")
                        {
                            temp = temp + "\n武器：" + Game.eqi[Convert.ToInt32(weapon.Substring(0, weapon.Length - 2))].name//装备名称
                            + "+" + Convert.ToInt64(weapon.Substring(weapon.Length - 2, 2)).ToString();//装备强化等级
                        }
                        if (armor != "")
                        {
                            temp = temp + "\n护甲：" + Game.eqi[Convert.ToInt32(armor.Substring(0, armor.Length - 2))].name//装备名称
                            + "+" + Convert.ToInt64(armor.Substring(armor.Length - 2, 2)).ToString();//装备强化等级
                        }
                        if (ornament != "")
                        {
                            temp = temp + "\n饰品：" + Game.eqi[Convert.ToInt32(ornament.Substring(0, ornament.Length - 2))].name//装备名称
                            + "+" + Convert.ToInt64(ornament.Substring(ornament.Length - 2, 2)).ToString();//装备强化等级
                        }

                        e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + temp);//发送装备信息
                    }

                    //查看背包操作------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy 查看背包")
                    {
                        inv.Clear();//清空inv列表
                        string temp = save.getkey(e.FromQQ.Id.ToString(), "Inventory", "") ;
                        if (temp == "") { e.FromGroup.SendGroupMessage("你的背包空空如也，快去获取装备吧！"); }
                        else
                        {
                            Inventoryload(e.FromQQ.Id);
                            temp = "";
                            for (int i = 0; i < inv.Count; i++)
                            {
                                temp = temp + (i+1).ToString()+"."+ Game.eqi[Convert.ToInt32(inv[i].id)].name;//Game.装备数据列表[背包列表[项数].ID].名称   
                                temp = temp +"+"+ inv[i].level.ToString()+"  ";//附上强化等级
                            }
                            inv.Clear();//清空inv列表
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id)+temp);
                        }
                    }

                    //穿上装备操作------------------------------------------------------------------------------------------
                    if (e.Message.Text.StartsWith("dy 装备"))
                    {
                        string[] temp = e.Message.Text.Split(' ');//以空格为界限进行分割
                        if (temp.Length > 2 )
                        {
                            Int32 tempi = 0;
                            while(temp[2].Length > tempi)//检测是否都为数字
                            {
                                tempi++;
                                if (  IsNumberic(temp[2].Substring(tempi-1,1))==false )//若否则停止
                                {
                                    e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id)+"输入内容错误!");
                                    break;
                                }
                                else if(temp[2].Length == tempi)//若最后一个字符也是数字则继续
                                {
                                    Inventoryload(e.FromQQ.Id);//加载物品栏
                                    Int32 temp2count =Convert.ToInt32 (temp[2]);//玩家输入的需要变动装备的序号
                                    Int32 ID = 0;
                                    if (temp2count <= inv.Count) //如果拥有这件装备执行下列代码
                                    {
                                        ID = Convert.ToInt32(inv[temp2count-1].id); //背包中对应需要变动装备的ID
                                        if (Game.eqi[ID].kind == "武器")//武器
                                        {
                                            if (save.getkey(e.FromQQ.Id.ToString(), "weapon", "none") != "none")//如果原先有装备则退回装备到背包
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "Inventory", save.getkey(e.FromQQ.Id.ToString(),
                                                    "Inventory", "")+ save.getkey(e.FromQQ.Id.ToString(), "weapon", "none")+",");
                                            }
                                            if (inv[temp2count - 1].level < 10)
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "weapon", ID.ToString()+ "0" + inv[temp2count - 1].level.ToString());
                                            }
                                            else
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "weapon", ID.ToString() + inv[temp2count - 1].level.ToString());
                                            }
                                        }
                                        else if (Game.eqi[ID].kind == "护甲")//护甲
                                        {
                                            if (save.getkey(e.FromQQ.Id.ToString(), "armor", "none") != "none")//如果原先有装备则退回装备到背包
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "Inventory", save.getkey(e.FromQQ.Id.ToString(),
                                                    "Inventory", "") + save.getkey(e.FromQQ.Id.ToString(), "armor", "none") + ",");
                                            }
                                            if (inv[temp2count - 1].level < 10)
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "armor", ID.ToString() + "0" + inv[temp2count - 1].level.ToString());
                                            }
                                            else
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "armor", ID.ToString() + inv[temp2count - 1].level.ToString());
                                            }
                                        }
                                        else if(Game.eqi[ID].kind == "饰品")//饰品
                                        {
                                            if (save.getkey(e.FromQQ.Id.ToString(), "ornament", "none") != "none")//如果原先有装备则退回装备到背包
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "Inventory", save.getkey(e.FromQQ.Id.ToString(),
                                                    "Inventory", "") + save.getkey(e.FromQQ.Id.ToString(), "ornament", "none") + ",");
                                            }
                                            if (inv[temp2count - 1].level < 10)
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "ornament", ID.ToString() + "0" + inv[temp2count - 1].level.ToString());
                                            }
                                            else
                                            {
                                                save.putkey(e.FromQQ.Id.ToString(), "ornament", ID.ToString() + inv[temp2count - 1].level.ToString());
                                            }
                                        }       
                                        inv.Clear();//清理inv列表
                                        DeleteEquipment(e.FromQQ.Id, Convert.ToInt32(temp[2]) - 1);//删除装备
                                        e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "装备成功!");
                                    }
                                    else { e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "你没有对应序号的装备！"); }
                                    inv.Clear();//清理inv列表
                                }
                            }
                            
                        }
                        else { e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "输入格式错误!"); }
                    }

                    //强化装备操作------------------------------------------------------------------------------------------
                    if(e.Message.Text == "dy 强化武器")
                    {
                        Int64 tempcoin=Convert.ToInt64(save.getkey(e.FromQQ.Id.ToString(),"coin","0"));//读取玩家硬币
                        string temp= save.getkey(e.FromQQ.Id.ToString(), "weapon", "none");//读取玩家武器到temp
                        if(temp != "none")
                        {
                            Int32 templevel = Convert.ToInt32 (temp.Substring(temp.Length - 2, 2));//取出强化等级
                            if(tempcoin >= Convert.ToInt64(200 * Math.Pow(1.6, templevel)))//如果钱够
                            {
                                addcoin(e.FromQQ.Id, -Convert.ToInt64(200 * Math.Pow(1.6, templevel)));//扣钱
                                templevel++;//等级+1
                                save.putkey(e.FromQQ.Id.ToString(),"weapon",temp.Substring(0,temp.Length-2)+templevel.ToString("00"));                                                          
                                tempcoin = Convert.ToInt64(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));//读取玩家硬币
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id)+ "强化成功！剩余硬币："+tempcoin.ToString());//发送消息
                            }
                            else { e.FromGroup.SendGroupMessage("硬币不足，需要：" + Convert.ToInt64(200 * Math.Pow(1.6, templevel)).ToString()); }//钱不够
                        }
                        else { e.FromGroup.SendGroupMessage("未装备武器！"); }//未装备武器
                    }
                    else if(e.Message.Text == "dy 强化护甲")
                    {
                        Int64 tempcoin = Convert.ToInt64(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));//读取玩家硬币
                        string temp = save.getkey(e.FromQQ.Id.ToString(), "armor", "none");//读取玩家护甲到temp
                        if (temp != "none")
                        {
                            Int32 templevel = Convert.ToInt32(temp.Substring(temp.Length - 2, 2));//取出强化等级
                            if (tempcoin >= Convert.ToInt64(200 * Math.Pow(1.6, templevel)))//如果钱够
                            {
                                addcoin(e.FromQQ.Id, -Convert.ToInt64(200 * Math.Pow(1.6, templevel)));//扣钱
                                templevel++;//等级+1
                                save.putkey(e.FromQQ.Id.ToString(), "armor", temp.Substring(0, temp.Length - 2) + templevel.ToString("00"));
                                tempcoin = Convert.ToInt64(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));//读取玩家硬币
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "强化成功！剩余硬币：" + tempcoin.ToString());//发送消息
                            }
                            else { e.FromGroup.SendGroupMessage("硬币不足，需要：" + Convert.ToInt64(200 * Math.Pow(1.6, templevel)).ToString()); }//钱不够
                        }
                        else { e.FromGroup.SendGroupMessage("未装备护甲！"); }//未装备护甲
                    }
                    else if (e.Message.Text == "dy 强化饰品")
                    {
                        Int64 tempcoin = Convert.ToInt64(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));//读取玩家硬币
                        string temp = save.getkey(e.FromQQ.Id.ToString(), "ornament", "none");//读取玩家饰品到temp
                        if (temp != "none")
                        {
                            Int32 templevel = Convert.ToInt32(temp.Substring(temp.Length - 2, 2));//取出强化等级
                            if (tempcoin >= Convert.ToInt64(200 * Math.Pow(1.6, templevel)))//如果钱够
                            {   
                                addcoin(e.FromQQ.Id, -Convert.ToInt64(200 * Math.Pow(1.6, templevel)));//扣钱                           
                                templevel++;//等级+1
                                save.putkey(e.FromQQ.Id.ToString(), "ornament", temp.Substring(0, temp.Length - 2) + templevel.ToString("00"));
                                tempcoin = Convert.ToInt64(save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));//读取玩家硬币
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(e.FromQQ.Id) + "强化成功！剩余硬币：" + tempcoin.ToString());//发送消息
                            }
                            else { e.FromGroup.SendGroupMessage("硬币不足，需要：" + Convert.ToInt64(200 * Math.Pow(1.6, templevel)).ToString()); }//钱不够
                        }
                        else { e.FromGroup.SendGroupMessage("未装备饰品！"); }//未装备饰品
                    }


                    //瑶光祝福----------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy 瑶光祝福")
                    {
                        e.FromGroup.SendGroupMessage("向百年来最伟大的术士支付一定的费用，即可获取瑶光神星的祝福力量！当前祝福等级为：" +
                            save.getkey(e.FromQQ.Id.ToString(), "Alkaid", "0")+"。\nTIP：输入dy 升级祝福 可以提升当前祝福的等级，每一" +
                            "级祝福需要消耗当前祝福等级*1000硬币，祝福最高等级不能超过自身等级！每一级*永久*增加3hp,2mp,1atk,0.5def,0.2aig,同时签" +
                            "到额外获得0.5体力。");
                    }
                    else if(e.Message.Text == "dy 升级祝福")
                    {
                        Player p = new Player(e.FromQQ.Id);//加载玩家数据
                        Int32 Alkaidlevel = Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(), "Alkaid", "0"));//读取瑶光祝福等级
                        if ( Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(),"Alkaid","0")) < p.level && //条件判断
                            Convert.ToInt32(save.getkey(e.FromQQ.Id.ToString(),"coin","0"))>= Alkaidlevel*1000)
                        {
                            addcoin(e.FromQQ.Id, -Alkaidlevel * 1000);//扣除金币
                            Alkaidlevel++;//等级+1
                            save.putkey(e.FromQQ.Id.ToString(), "Alkaid", Alkaidlevel.ToString());//储存瑶光祝福等级
                            e.FromGroup.SendGroupMessage("伴随着耀眼的光芒，瑶光祝福的力量加强了：" + Alkaidlevel.ToString() + "级。当" +
                                "前硬币还剩下：" + save.getkey(e.FromQQ.Id.ToString(), "coin", "0"));
                        }
                        else { e.FromGroup.SendGroupMessage("硬币不足或祝福等级不能超过自身等级！"); }
                    }

                    //VIP系统
                    if(e.Message.Text =="dy vip")
                    {
                        e.FromGroup.SendGroupMessage("VIP获取通道关闭");
                    }

                    //挑战副本操作------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy 挑战1") fight(numrnd(10) - 1, e);
                    if (e.Message.Text == "dy 挑战2") fight(numrnd(10) + 9, e);
                    if (e.Message.Text == "dy 挑战3") fight(numrnd(10) + 19,e);
                    if (e.Message.Text == "dy 挑战4") fight(numrnd(10) + 29,e);
                    if (e.Message.Text == "dy 挑战5") fight(numrnd(10) + 39, e);

                    //在线PVP操作（不添加经验和硬币，这个等你回来再说）---------------------------------------------------------
                    if (e.Message.Text.StartsWith("dy pvp"))
                    {
                        fight(long.Parse(e.Message.Text.Replace("dy pvp","").Replace("[CQ:at,qq=", "").Replace("]", "").Trim()), e);
                    }

                    //更新公告
                    if(e.Message.Text == "dy 更新公告")
                    {
                        e.FromGroup.SendGroupMessage
                            (
                                "1：强化系统上线！！！！\n" +
                                "2：修复了强化消耗硬币错误的bug（感谢玩家王铜反馈的bug）\n" +
                                "3：副本6策划中~~~~\n" +
                                "4：经过数值推算，新增效果瑶光增加每一级防御+0.5\n" +
                                "5：重要通告：目前暂停赞助！已赞助玩家不影响\n"+
                                "更新日期:2020年8月2日"
                            );
                    }

                    //重要通告
                    if(e.Message.Text == "dy 通告")
                    {
                        e.FromGroup.SendGroupMessage("由于tx限制了同平台所有机器人框架，dy将可能会被不可抗力暂停。" +
                            "\n所有数据保留，考虑会开发独立应用，目前机器人尚可运行！请勿担心！");
                    }
                }

                //指令帮助操作------------------------------------------------------------------------------------------
                if (e.Message.Text == "dy 帮助")
                {
                    if (e.FromQQ.Id.ToString() != save.getkey(e.FromQQ.Id.ToString(), "account", ""))
                    {
                        e.FromGroup.SendGroupMessage("检测到您尚未注册帐号，请发送：\ndy 注册");
                    }
                    else
                    {
                        e.FromGroup.SendGroupMessage
                            (
                                "dy 注册：注册账号\n" +
                                "dy 签到：签到领硬币和活力\n" +
                                "dy 查看硬币：查看你持有的硬币数\n" +                     
                                "dy 查看属性：查看你的战斗属性\n" +                  
                                "dy 查看装备：查看你持有的装备\n" +
                                "dy 挑战1/2/3/4/5：消耗活力挑战一次副本\n" +
                                "dy 更新公告：查看最近一次更新内容\n"+
                                "dy 查看背包：查看你背包内的装备\n"+
                                "dy 装备 数字：装备对应序号的装备\n"+
                                "dy 强化武器/护甲/饰品：强化装备\n" +
                                "dy 瑶光祝福：查看瑶光祝福等级和提示\n" +
                                "dy vip：查看VIP详细\n"+
                                "dy 通告：查看*重要*通告"
                            );
                    }
                }

                //以下操作只有开发者才可以生效
                if (e.FromQQ.Id == 2584036741)
                {
                    //手动更新系统时间-------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy sdate") //载入系统时间
                    {
                        string sdate;//读取日期存档
                        sdate = DateTime.Now.ToString("yyyy-MM-dd");
                        save.putkey("administrator", "sdate", sdate);
                        e.FromGroup.SendGroupMessage(CQApi.CQCode_At(2584036741) + "系统日期载入完成,当前日期：" + sdate);
                    }
                    //输出数据文本-----------------------------------------------------------------------------------------------
                    if (e.Message.Text == "dy data")//生成当前存档数据模块
                    {
                        string test = ""; int userindex = 0;
                        test = "存档主人：" + save.data.Owner + "\r\n";
                        //foreach(变量声明 in 一个集合)，这样你就可以遍历到这个集合里面的所有元素，这些元素将复制到你声明的变量
                        foreach (Storage.DataArea user in save.data.Areas)
                        {
                            //遍历了save.data.Areas内所有的Storage.DataArea类型的成员
                            //然后存在了user，然后Storage.DataArea又有两个成员，一个叫Items，是这个用户的所有key的集合，一个叫User，就是这个用户的名字
                            test += "用户(" + userindex + ")：" + user.User + "\r\n";
                            int itemindex = 0;
                            //这里遍历了user.Items内所有的Storage.DataItem类型的成员
                            //然后存在了item，然后Storage.DataItem这个类型也有两个成员，一个叫Key，一个叫Value
                            foreach (Storage.DataItem item in user.Items)
                            {
                                test += "┗━━━━" + item.Key + "：" + item.Value + "\r\n";
                                itemindex++;
                            }
                            userindex++;
                        }
                        File.WriteAllText(@"C:\Users\Administrator\Desktop\DYdata.txt", test);
                        e.FromGroup.SendGroupMessage("Data has been output！");
                    }
                    //VIP等级修改-----------------------------------------------------------------------------------------------
                    if(e.Message.Text.StartsWith("dy vipadd ")) 
                    { 
                        string temp= e.Message.Text.Substring(10, e.Message.Text.Length - 10);//取出QQ号
                        Int32 viprank =Convert.ToInt32( save.getkey(temp, "VIPrank", "0"));//取出VIP等级
                        save.putkey(temp, "VIPrank", (viprank+1).ToString());//储存VIP等级+1
                        e.FromGroup.SendGroupMessage("操作成功！");
                    }
                }

                //测试---------------------------------------------------------------------------------
                if (e.Message.Text == "dy") { e.FromGroup.SendGroupMessage("我在呐！"); }
                if (e.FromQQ.Id == 2584036741)
                {
                    if (e.Message.Text == "dy quitest") e.FromGroup.SendGroupMessage(Game.eqi[25].name);
                    if (e.Message.Text == "dy test") { e.FromGroup.SendGroupMessage("DreamY is ready!"); }
                    if (e.Message.Text == "dy rnd") { e.FromGroup.SendGroupMessage(rnd(100, 50).ToString()); }
                    if (e.Message.Text == "dy numrnd") { e.FromGroup.SendGroupMessage(numrnd(4).ToString()); }
                    if (e.Message.Text == "dy monster") { e.FromGroup.SendGroupMessage(Game.mon[1].agi.ToString()); }
                    if (e.Message.Text == "dy testfight") { fight(0, e); }
                    if (e.Message.Text == "dy addexp") { addexp(e.FromQQ.Id, 100); }
                    if (e.Message.Text == "dy agitest") 
                    { Player p = new Player(e.FromQQ.Id);e.FromGroup.SendGroupMessage(p.AAGI.ToString()+" "+p.OAGI.ToString()+" "+p.WAGI.ToString()); }
                }
            }
            catch(Exception err)
            {
                string errreport = "/// DreamY 故障报告 ///\n故障时间：" + DateTime.Now.ToString() +
                                   "\n调用堆栈：\n" + err.StackTrace + "\n故障提示：" + err.Message +
                                   "\n--------------\n故障触发者：QQ" + e.FromQQ.Id + "\n" +
                                   "故障来源群：" + e.FromGroup.Id + "\n故障诱发消息：\n" + e.Message.Text;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errreport);
                Console.ForegroundColor = ConsoleColor.Gray;
                e.FromGroup.SendGroupMessage(errreport);
                new QQ(e.CQApi, 2584036741).SendPrivateMessage(errreport);
            }
        }
    }
}
