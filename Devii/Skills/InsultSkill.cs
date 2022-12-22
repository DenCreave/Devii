using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.StatusAilments;

namespace Devii.Skills;

public class InsultSkill : ISkill
{
    public string Title => "Insult";

    private string[] _insults = 
    {
        "That's a face only a mother could love.","Szop-o-matic", "I thought ogres are only a myth!"
        , "Aaaah this smell, you know modern society can help with it", "No! Santa Claus doesn't exist!", "No, you dont need more rights, you need therapy."
        , "Damn you're so hairy, you speak wookie right? whrüüüü","You look just like a condom commercial" ,"What is this nightmare that's staring at me"
    };

    private string[] _sufferings =
    {
        "Ouch, this huuuurts", "Mamaaaaa Q.Q im being huuurt", "No bully please, leave me alone!"
        ,"Im so hurt, i criie qqqqqqq","YU LIEEE, NOOO, KANT BEE!","why u do dis to me? T-T"
        ,"mamaaaaa mamaaaaa T_T_T_T_TT_T", "Im just only saying ,- FUCK YUUUU", "I dyont even deserwe dis T_T_T_T"
    };
    
    public bool AffectedByGCD => true;
    public int? Value => 3;
    public double EffectiveRate => 0.3;
    public bool IsHarmful => true;
    public string Description => "Insulting the target 3 times over 6 seconds." +
                                 " The way this works is you and the target both get buff, your buff gonna change your text you say " +
                                 "and your targets buff gon get his changed and take damage";
    

    public void RequestAction(Creature self, Creature target)
    {
        SkillQue queMe = new SkillQue(this, self, target);
        self.SkillQues.Enqueue(queMe);
    }

    
    public void CastMe(Creature self, Creature target)
    {
        InsultDebuff debuff = new InsultDebuff();

        Random rnd = new Random();

        string[] insults = new string[3];
        string[] sufferings = new string[3];

        for (int i = 0; i < 3; i++)
        {
            insults[i] = _insults[ rnd.Next(_insults.Length)];
            sufferings[i] = _sufferings[ rnd.Next(_sufferings.Length)];
        }
        
        InsultSpeechAilment selfSpeechAilment = new InsultSpeechAilment(insults);
        InsultSpeechAilment targetSpeechAilment = new InsultSpeechAilment(sufferings);
        
        debuff.RequestAction(self,target);
        selfSpeechAilment.RequestAction(self,target);
        targetSpeechAilment.RequestAction(target,target);
        
        self.SetLastGCDTrigger();
    }
    
    
}