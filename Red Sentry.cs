using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Unity.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Il2CppAssets.Scripts.Unity;

namespace SentriesAsTowers
{
    public class RedSentry : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Support;

        public override string BaseTower => TowerType.Sentry;
        public override int Cost => 250;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 0;
        public override string Portrait => "RedSentryPortrait";
        public override string Icon => "RedSentryPortrait";
        public override string Description => "The Engineer's sentry as a tower.";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.isSubTower = false;

            towerModel.RemoveBehavior<TowerExpireModel>();
            towerModel.RemoveBehavior<SavedSubTowerModel>();

            towerModel.GetAttackModel().GetBehavior<TargetFirstModel>().isOnSubTower = false;
            towerModel.GetAttackModel().GetBehavior<TargetLastModel>().isOnSubTower = false;
            towerModel.GetAttackModel().GetBehavior<TargetCloseModel>().isOnSubTower = false;
            towerModel.GetAttackModel().GetBehavior<TargetStrongModel>().isOnSubTower = false;

            towerModel.footprint.doesntBlockTowerPlacement = false;
            towerModel.GetDescendant<CircleFootprintModel>().radius = 2;
        }

        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.EngineerMonkey).towerIndex + 1;
        }
    }

    public class LargerServiceAreaRed : ModUpgrade<RedSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 250;

        public override string Icon => "LargerServiceAreaIcon";
        public override string DisplayName => "Larger Service Area";
        public override string Description => "Sentry has longer range.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.range = 56;
            towerModel.GetAttackModel().range = 56;
        }
    }

    public class DeconstructionRed : ModUpgrade<RedSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 350;

        public override string Icon => "DeconstructionIcon";
        public override string DisplayName => "Deconstruction";
        public override string Description => "Sentry shots do extra damage to MOAB-class and fortified Bloons.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 1, false, false) { name = "MoabModifier_" });
        }
    }

    public class SprocketsRed : ModUpgrade<RedSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 575;

        public override string Icon => "SprocketsIcon";
        public override string DisplayName => "Sprockets";
        public override string Description => "Increases sentry gun attack speed.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].rate = 0.475f;
        }
    }

    public class CrushingSentry : ModUpgrade<RedSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 1250;

        public override string Portrait => "SentryCrushingPortrait";
        public override string Icon => "SentryCrushingPortrait";
        public override string DisplayName => "Crushing Sentry";
        public override string Description => "Sentry gun becomes a crushing sentry.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var crushingSentry = Game.instance.model.GetTowerFromId("EngineerMonkey-420").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateTypedTowerModel>().crushingTower.GetAttackModel().Duplicate();
            crushingSentry.GetBehavior<TargetFirstModel>().isOnSubTower = false;
            crushingSentry.GetBehavior<TargetLastModel>().isOnSubTower = false;
            crushingSentry.GetBehavior<TargetCloseModel>().isOnSubTower = false;
            crushingSentry.GetBehavior<TargetStrongModel>().isOnSubTower = false;

            towerModel.RemoveBehavior(towerModel.GetAttackModel());
            towerModel.AddBehavior(crushingSentry);
        }
    }

    public class RedSentryParagon : ModUpgrade<RedSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 75000;

        public override string Portrait => "SentryParagonRedPortrait";
        public override string Icon => "SentryParagonRedPortrait";
        public override string DisplayName => "Sentry Paragon Red";
        public override string Description => "Transforms the sentry gun into the red paragon sentry.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.display = new() { guidRef = "af6bc5f76310fa84eae188f2f5381dc6" };

            var plasmaSentry = Game.instance.model.GetTowerFromId("SentryParagonRed").GetAttackModel().Duplicate();
            var redChildSentry = Game.instance.model.GetTowerFromId("SentryParagonRed").GetAttackModel(1).Duplicate();

            plasmaSentry.AddBehavior(new TargetFirstModel("", true, false));
            plasmaSentry.AddBehavior(new TargetLastModel("", true, false));
            plasmaSentry.AddBehavior(new TargetCloseModel("", true, false));
            plasmaSentry.AddBehavior(new TargetStrongModel("", true, false));

            towerModel.RemoveBehavior(towerModel.GetAttackModel());
            towerModel.AddBehavior(plasmaSentry);
            towerModel.AddBehavior(redChildSentry);

            towerModel.range = plasmaSentry.range;
        }
    }
}
