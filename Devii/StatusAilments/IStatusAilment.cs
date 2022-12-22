using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.Enums;

namespace Devii.StatusAilments;

public interface IStatusAilment
{
     string Name { get; }
     int DurationMillisec { get; }
     int? MaxTicks { get; }
     int CurrentTicks { get; set; } //duration/maxticks*currentticks -> current tick initializes at 1 except for ones that do initial action. 
     bool IsHarmful { get; }
     bool IsDisplayed { get; } //should it show up on screens buff list with barchart?
     StatusAilmentTypes[] Types { get; }
     DateTime TimeOfAcquisition { get; init; }
     
     void RequestAction(Creature self, Creature target);

     void Activate(Creature self, Creature target);
     
     void TakeAction(Creature self, Creature target);  
     
     void TakeAction(Creature self, Creature target, ref double? value);
     void TakeAction(Creature self, Creature target, string value);

     void Deactivate(Creature self, Creature target); 


}