using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.Enums;
using Devii.Exceptions;

namespace Devii.StatusAilments;

public sealed class InsultSpeechAilment : IStatusAilment
{
    public string Name => "Insult Texts";
    public int DurationMillisec => 6000;
    public int? MaxTicks => 3;
    public int CurrentTicks { get; set; }
    public bool IsHarmful => false;
    public bool IsDisplayed => false;
    public string[] Speech { get; set;}
    public int SpeechIndex { get; set; } 
    public StatusAilmentTypes[] Types => new[] { StatusAilmentTypes.OverTimeEffect };
    public DateTime TimeOfAcquisition { get; init; }

    public InsultSpeechAilment(string[] speechList)
    {
        Speech = (string[]) speechList.Clone();
        CurrentTicks = 0;
        SpeechIndex = 0;
        TimeOfAcquisition = new DateTime();
        TimeOfAcquisition = DateTime.Now;
        
    }
    
    public void RequestAction(Creature self, Creature target)
    {
        StatusAilmentQue ailmentQue = new StatusAilmentQue(this, self, target);
        self.StatusAilmentQues.Enqueue(ailmentQue);
    }

    public void Activate(Creature self, Creature target)
    {
        self.StatusAilments.AddOrUpdate(Name,this, (key,value) =>
        {
            return this;
        });
    }

    public void TakeAction(Creature self, Creature target)
    {
        if (SpeechIndex<MaxTicks)
        {
            self.SetSpeech(Speech[SpeechIndex]);
            ++SpeechIndex;
            ++CurrentTicks;
        }
        else
        {
            if (!String.IsNullOrEmpty(self.GetSpeechBox()))
            {
                self.SetSpeech(String.Empty);   
            }
        }
    }

    public void TakeAction(Creature self, Creature target, ref  double? value)
    {
        throw new BuffTakeActionError(Name);
    }

    public void TakeAction(Creature self, Creature target, string value)
    {
        throw new BuffTakeActionError(Name);
    }

    public void Deactivate(Creature self, Creature target)
    {
        self.StatusAilments.TryRemove(Name, out _);
    }
}