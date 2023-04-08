using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace PersonalizedLogisticsMechanoid
{
    public enum BALANCE_PRESETS
    {
        DEFAULT,
        ALTERNATIVE,
    }

    public class PersonalizedLogisticsMechanoidSettings : ModSettings
    {
        public Dictionary<string, float> stats = PersonalizedLogisticsMechanoidMod.GetStatsOf(
            BALANCE_PRESETS.DEFAULT
        );

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref stats, "stats");
            base.ExposeData();
        }
    }

    public class PersonalizedLogisticsMechanoidMod : Mod
    {
        PersonalizedLogisticsMechanoidSettings settings;

        public PersonalizedLogisticsMechanoidMod(ModContentPack content)
            : base(content)
        {
            this.settings = GetSettings<PersonalizedLogisticsMechanoidSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("(!) Restarting the game is necessary to apply the changes.");
            listingStandard.Gap(24f);

            settings.stats["MoveSpeed"] = AddSlider(
                ls: listingStandard,
                label: "Move speed: ",
                value: settings.stats["MoveSpeed"],
                range: new float[] { 0.0f, 6.0f }
            );

            settings.stats["BandwidthCost"] = AddSlider(
                ls: listingStandard,
                label: "Bandwidth cost: ",
                value: settings.stats["BandwidthCost"],
                range: new float[] { 1.0f, 6.0f },
                roundingFactor: 1f
            );

            settings.stats["CaravanRidingSpeedFactor"] = AddSlider(
                ls: listingStandard,
                label: "Caravan riding speed factor: ",
                value: settings.stats["CaravanRidingSpeedFactor"],
                range: new float[] { 1.0f, 4.0f }
            );

            settings.stats["baseBodySize"] = AddSlider(
                ls: listingStandard,
                label: "Mechanoid’s base body size: ",
                value: settings.stats["baseBodySize"],
                range: new float[] { 0.2f, 28.0f }
            );
            listingStandard.Indent();
            listingStandard.Label(
                "* Caravan carry weight will be: "
                    + (settings.stats["baseBodySize"] * 35 - 5)
                    + "kg"
            );
            listingStandard.Outdent();
            listingStandard.Gap(24f);

            listingStandard.Indent(inRect.width / 2.0f);
            listingStandard.Label("Reset all to:");

            if (listingStandard.ButtonText(label: "Original balance", widthPct: 0.2f))
            {
                settings.stats = GetStatsOf(BALANCE_PRESETS.DEFAULT);
            }

            if (listingStandard.ButtonText(label: "Alternative balance", widthPct: 0.2f))
            {
                settings.stats = GetStatsOf(BALANCE_PRESETS.ALTERNATIVE);
            }

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Logistics mechanoid";
        }

        private float AddSlider(
            Listing_Standard ls,
            string label,
            float value,
            float[] range,
            float roundingFactor = 10.0f
        )
        {
            return Mathf.Round(
                    ls.SliderLabeled(
                        label + Mathf.Round(value * roundingFactor) / roundingFactor,
                        value,
                        range[0],
                        range[1]
                    ) * roundingFactor
                ) / roundingFactor;
        }

        public static Dictionary<string, float> GetStatsOf(BALANCE_PRESETS balancePreset)
        {
            var stats = new Dictionary<string, float>();

            switch (balancePreset)
            {
                case BALANCE_PRESETS.ALTERNATIVE:
                    stats["MoveSpeed"] = 1.9f;
                    stats["BandwidthCost"] = 3.0f;
                    stats["CaravanRidingSpeedFactor"] = 2.0f;
                    stats["baseBodySize"] = 8.4f;
                    break;
                case BALANCE_PRESETS.DEFAULT:
                    stats["MoveSpeed"] = 2.7f;
                    stats["BandwidthCost"] = 2.0f;
                    stats["CaravanRidingSpeedFactor"] = 1.6f;
                    stats["baseBodySize"] = 4.0f;
                    break;
            }

            return stats;
        }
    }
}
