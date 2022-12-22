using System.Reflection;

namespace Devii.Skills;

public sealed class SkillLoader
{
    public Dictionary<string , ISkill> Skills { get; }

    public SkillLoader()
    {
        Skills = new();
        
        Assembly asm= Assembly.GetAssembly(typeof(SkillLoader));
        if (asm==null)
        {
            throw new NullReferenceException("Skill loader assembly was NULL");
        }

        var types = asm.GetTypes()
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.IsAssignableTo(typeof(ISkill)));

        try
        {
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is ISkill skill)
                {
                    Skills.Add(skill.Title, skill);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}