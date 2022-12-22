using System.Collections.Concurrent;
using System.Diagnostics;
using Devii.Containers;
using Devii.Enums;
using Devii.Exceptions;
using Devii.Handlers;
using Devii.Skills;
using Devii.StatusAilments;

namespace Devii.Creatures;

public sealed class Creature
{
    public string Name { get; init; }
    public string ProfilePic { get; }
    
    public string[] SkillKeysOrdered { get; }
    public int BaseGlobalCoolDownMs { get; init; } 
    public bool IsAutoAttacking { get; init; }
    public string SpeechBox;
    public int MaxHealth;
    public int Health;
    public int Strength;
    public double Haste;//value is 1 on default, with 50% increase it gets 0.5, 0 mean no gcd at all
    //for methods like these ill make a setter so it doesnt go below 0
    public double DamageDoneMultiplier;
    public double DamageTakenMultiplier; //default = 1; doesnt go below zero 
    public double HealingDoneMultiplier;
    public double HealingTakenMultiplier;

    // default is false, set 1 for true.
    private int _threadSafeBoolBackValue = 0;

    
    public bool IsInvicible
    {
        get { return (Interlocked.CompareExchange(ref _threadSafeBoolBackValue, 1, 1) == 1); }
        set
        {
            if (value) Interlocked.CompareExchange(ref _threadSafeBoolBackValue, 1, 0);
            else Interlocked.CompareExchange(ref _threadSafeBoolBackValue, 0, 1);
        }
    }
    
    public long LastGCDTrigger;

    public Creature Target { get; set; }

    public CreatureLog PersonalCombatLog { get; set; }
    public ConcurrentQueue<StatusAilmentQue> StatusAilmentQues { get; set; }
    public ConcurrentQueue<SkillQue> SkillQues { get; set; }
    public ConcurrentDictionary<string,IStatusAilment> StatusAilments { get; set; }
    public ConcurrentDictionary<string,ISkill> SkillLists { get; set; }

    
    public Creature(string name, int baseGlobalCoolDownMs, bool isAutoAttacking, int maxHealth, int strength, double haste,ConcurrentDictionary<string,ISkill> skillLists, string profilePic)
    {
        Name = name;
        BaseGlobalCoolDownMs = baseGlobalCoolDownMs;
        IsAutoAttacking = isAutoAttacking;
        MaxHealth = maxHealth;
        Health = MaxHealth;
        Strength = strength;
        Haste = haste;

        DamageDoneMultiplier = 1;
        DamageTakenMultiplier = 1;
        HealingDoneMultiplier = 1;
        HealingTakenMultiplier = 1;

        PersonalCombatLog = new CreatureLog();
        StatusAilmentQues = new ConcurrentQueue<StatusAilmentQue>();
        SkillQues = new ConcurrentQueue<SkillQue>();
        StatusAilments = new ConcurrentDictionary<string, IStatusAilment>();
        SkillLists = new ConcurrentDictionary<string, ISkill>(skillLists);

        SkillKeysOrdered=SkillLists.Keys.OrderDescending().ToArray();
        
        SetLastGCDTrigger();
        IsInvicible = false;


        ProfilePic = profilePic;

    }
    
    public double GetDamageDoneMultiplier()
    {
        return Interlocked.CompareExchange(ref DamageDoneMultiplier, 0, 0);
    }
    public void SetDamageDoneMultiplier( double value)
    {
        double tmp = Math.Max(GetDamageDoneMultiplier() + value,0);
        Interlocked.CompareExchange(ref DamageDoneMultiplier, tmp,DamageDoneMultiplier );
    }

    public double GetDamageTakenMultiplier()
    {
        return Interlocked.CompareExchange(ref DamageTakenMultiplier, 0, 0);
    }
    
    public void SetDamageTakenMultiplier( double value)
    {
        double tmp = Math.Max(GetDamageTakenMultiplier() + value,0);
        Interlocked.CompareExchange(ref DamageTakenMultiplier, tmp,DamageTakenMultiplier );
    }

    public double GetHealingDoneMultiplier()
    {
        return Interlocked.CompareExchange(ref HealingDoneMultiplier, 0, 0);
    }

    
    public void SetHealingDoneMultiplier( double value)
    {
        double tmp = Math.Max(GetHealingDoneMultiplier() + value,0);
        Interlocked.CompareExchange(ref HealingDoneMultiplier, tmp,HealingDoneMultiplier );
    }
    
    
    public double GetHealingTakenMultiplier()
    {
        return Interlocked.CompareExchange(ref HealingTakenMultiplier, 0, 0);
    }
    
    public void SetHealingTakenMultiplier( double value)
    {
        double tmp = Math.Max(GetHealingTakenMultiplier() + value,0);
        Interlocked.CompareExchange(ref HealingTakenMultiplier, tmp,HealingTakenMultiplier );
    }
    
    

    public double GetHaste()
    {
        return Interlocked.CompareExchange(ref Haste, 0, 0);
    }
    
    public void SetHaste( double value)
    {
        double tmp = Math.Max(GetHaste() + value,0);
        Interlocked.CompareExchange(ref Haste, tmp,Haste );
    }
    
    public int GetStrength()
    {
        return Interlocked.CompareExchange(ref Strength, 0, 0);
    }
    
    public void SetStrength( int value)
    {
        int tmp = Math.Max(GetStrength() + value,0);
        Interlocked.CompareExchange(ref Strength, tmp,Strength );
    }
    public int GetMaxHealth()
    {
        return Interlocked.CompareExchange(ref MaxHealth, 0, 0);
    }


    public void SetLastGCDTrigger()
    {
        Interlocked.Exchange(ref LastGCDTrigger, DateTime.Now.ToBinary());
    }
    
    public DateTime GetLastGCDTrigger()
    {
        return DateTime.FromBinary(Interlocked.CompareExchange(ref LastGCDTrigger, 0, 0));
    }

    public void SetMaxHealth( int value)
    {
        int tmp = Math.Max(GetMaxHealth() + value,0);
        Interlocked.CompareExchange(ref MaxHealth, tmp,MaxHealth );
    }


    public string GetSpeechBox()
    {
        return Interlocked.CompareExchange(ref SpeechBox, "", "");
    }

    public void SetSpeech(string value)
    {
        Interlocked.CompareExchange(ref SpeechBox, value, SpeechBox);
    }
    

    public int GetHealth()
    {
        //return Thread.VolatileRead(ref Health);
        // john skeet says not to do this
        //@opc: I think the basic point is that it's guaranteed that anything else you read afterwards will be at least as fresh. So you can make sensible decisions, if you get your reads and writes in the right order. It's possible that the memory barrier is achieved precisely by reading/writing freshly - but it's not guaranteed by the spec :( – 
        //Jon Skeet
        //Nov 21, 2009 at 21:01
        return Interlocked.CompareExchange(ref Health, 0, 0);
    }

    public void SetHealth( int value)
    {
        int tmp = Math.Min(Math.Max(GetHealth() + value, 0), GetMaxHealth());
        Interlocked.CompareExchange(ref Health, tmp,Health );
    }


    public void TakeDmg(double? value)
    {
        double? damageTake = value * GetDamageTakenMultiplier();
        CheckForStatusAilment(StatusAilmentTypes.WhenStruck,"_NONE_", ref damageTake);
        CheckForStatusAilment(StatusAilmentTypes.Absorb,"_NONE_", ref damageTake);
        if (damageTake is not null  && damageTake !=0)
        {
            int dmg = (int)damageTake;
            SetHealth(-dmg);
            PersonalCombatLog.LogAction(Name,dmg,true);
        }
    }

    public void AilmentTakeAction(IStatusAilment  buff,string whichSkill, ref double? damageDeal) 
    {
        if (buff.Types.Contains(StatusAilmentTypes.ActionIntegerValueRequired))
        {
            buff.TakeAction(this,Target,ref  damageDeal );
            //what could take a value during initiating dmg when the class itself gets passed too?
            //todo, figure out where this fails
        }else if (buff.Types.Contains(StatusAilmentTypes.ActionStringValueRequired))
        {
            buff.TakeAction(this,Target,whichSkill);
        }
        else //else, call the default take action
        {
            buff.TakeAction(this,Target);
        }
    }

    public void CheckForStatusAilment(StatusAilmentTypes ailmentType, string whichSkill, ref double? damageDeal)
    {
        if (StatusAilments.Any( sa => sa.Value.Types.Contains(ailmentType)))
        {
            var buffs2Activate =
                StatusAilments.Values.Where(sa => sa.Types.Contains(ailmentType));

            foreach (var buff in buffs2Activate)
            {
                AilmentTakeAction(buff, whichSkill, ref damageDeal);
            }
        }

    }

    public double? DealDmg(string whichSkill)
    {

        if (SkillLists.Keys.Contains(whichSkill))
        {
            double? damageDeal = (SkillLists[whichSkill].Value is not null
                ? SkillLists[whichSkill].Value * SkillLists[whichSkill].EffectiveRate * GetStrength() * GetDamageDoneMultiplier()
                : 0);

            CheckForStatusAilment(StatusAilmentTypes.OnSkillUse, whichSkill, ref damageDeal);
            
            return damageDeal;
        }
        else
        {
            throw new DealDmgError(whichSkill);
            return null;
        }
    }

    public bool ActionRequester(string whichSkill)
    {
        
        if ( (DateTime.Now.Subtract(GetLastGCDTrigger()).TotalMilliseconds>BaseGlobalCoolDownMs*Haste || !SkillLists[whichSkill].AffectedByGCD  )
             && SkillLists.Keys.Contains(whichSkill))
        {
            SkillLists[whichSkill].RequestAction(this,Target);
            return true;
        }
        else if(!StatusAilments.IsEmpty)
        {
            var ailments = StatusAilments.Values;
            foreach (var buffDebuff in ailments)
            {
                if (buffDebuff.Types.Contains(StatusAilmentTypes.OnSkillUse) &&
                    buffDebuff.Types.Contains(StatusAilmentTypes.OffGlobalInstant))
                {
                    if (buffDebuff.Types.Contains(StatusAilmentTypes.ActionStringValueRequired))
                    {
                        buffDebuff.TakeAction(this, Target, whichSkill);
                    }
                    else
                    {
                        buffDebuff.TakeAction(this, Target);
                    }
                    return true;
                }

                return false;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public void TakeHealing(double? value)
    {
        double? healTake = value * GetDamageTakenMultiplier();
        if (healTake is not null  && healTake !=0)
        {
            int heal = (int)healTake;
            SetHealth(heal);
            PersonalCombatLog.LogAction(Name,heal,false);
        }
    }

    public double? DealHealing(string whichSkill)
    {
        if (SkillLists.Keys.Contains(whichSkill))
        {
            double? healindDone = (SkillLists[whichSkill].Value is not null 
                ? SkillLists[whichSkill].Value * GetStrength() * GetHealingDoneMultiplier()
                : 0);

            CheckForStatusAilment(StatusAilmentTypes.OnSkillUse, whichSkill, ref healindDone);
            

            return healindDone;
        }
        else
        {
            //todo log on error line, that it has no skill as such
            return null;
        }
    }
    
    
    public async Task<int> AutoAttack()
    {
        Random rnd = new Random();
        var skillOptions = SkillLists.Keys;
        await Task.Run(async () =>
        {


            while (IsAutoAttacking && GetHealth() > 0 && Target.GetHealth() > 0)
            {
                ActionRequester(skillOptions.ElementAt(rnd.Next(skillOptions.Count)));
                await Task.Delay(100);
            }

        });
        return GetHealth();
    }

    public async Task<int> ManualAttack()
    {
        int value = -1;
        await Task.Run(() =>
        {
            while (GetHealth() > 0 && Target.GetHealth() > 0)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                if (char.IsDigit(pressedKey.KeyChar))
                {
                    value = int.Parse(pressedKey.KeyChar.ToString());
                }
                else
                {
                    value = -1;
                }

                if (value > 0 && value < SkillKeysOrdered.Length + 1)
                {
                    ActionRequester(SkillKeysOrdered[value - 1]);
                }
            }
        });
        return GetHealth();
    }
    
    
}