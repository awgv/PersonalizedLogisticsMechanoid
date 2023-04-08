using System.Collections.Generic;
using System.Xml;
using Verse;

namespace PersonalizedLogisticsMechanoid
{
    public class PatchOperationUseSettings : PatchOperation
    {
        private List<string> ListOfPathsToExpose;

        private PersonalizedLogisticsMechanoidSettings settings = LoadedModManager
            .GetMod<PersonalizedLogisticsMechanoidMod>()
            .GetSettings<PersonalizedLogisticsMechanoidSettings>();

        protected override bool ApplyWorker(XmlDocument xml)
        {
            bool result = false;

            foreach (var path in ListOfPathsToExpose)
            {
                foreach (
                    var defNode in xml.SelectNodes(
                        "Defs/ThingDef[defName=\"Mech_Logistic\"]/" + path
                    )
                )
                {
                    result = true;

                    var xmlNode = defNode as XmlNode;

                    xmlNode.InnerText = settings.stats[xmlNode.Name].ToString();
                }
            }

            return result;
        }
    }
}
