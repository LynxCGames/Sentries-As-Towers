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
using Il2CppAssets.Scripts.Unity;

namespace SentriesAsTowers
{
    public class GreenSentry : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Support;

        public override string BaseTower => TowerType.Sentry;
        public override int Cost => 250;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 0;
        public override string Portrait => VanillaSprites.SentryPortrait;
        public override string Icon => VanillaSprites.SentryPortrait;
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

    public class LargerServiceAreaGreen : ModUpgrade<GreenSentry>
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

    public class DeconstructionGreen : ModUpgrade<GreenSentry>
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

    public class SprocketsGreen : ModUpgrade<GreenSentry>
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

    public class EnergySentry : ModUpgrade<GreenSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 1250;

        public override string Portrait => "SentryEnergyPortrait";
        public override string Icon => "SentryEnergyPortrait";
        public override string DisplayName => "Energy Sentry";
        public override string Description => "Sentry gun becomes an energy sentry.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var energySentry = Game.instance.model.GetTowerFromId("EngineerMonkey-420").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateTypedTowerModel>().energyTower.GetAttackModel().Duplicate();
            energySentry.GetBehavior<TargetFirstModel>().isOnSubTower = false;
            energySentry.GetBehavior<TargetLastModel>().isOnSubTower = false;
            energySentry.GetBehavior<TargetCloseModel>().isOnSubTower = false;
            energySentry.GetBehavior<TargetStrongModel>().isOnSubTower = false;

            towerModel.RemoveBehavior(towerModel.GetAttackModel());
            towerModel.AddBehavior(energySentry);
        }
    }

    public class GreenSentryParagon : ModUpgrade<GreenSentry>
    {
        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 55000;

        public override string Portrait => "SentryParagonGreenPortrait";
        public override string Icon => "SentryParagonGreenPortrait";
        public override string DisplayName => "Sentry Paragon Green";
        public override string Description => "Transforms the sentry gun into the green paragon sentry.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.display = new() { guidRef = "af6bc5f76310fa84eae188f2f5381dc6" };

            var laserSentry = Game.instance.model.GetTowerFromId("SentryParagonGreen").GetAttackModel().Duplicate();
            var greenChildSentry = Game.instance.model.GetTowerFromId("SentryParagonGreen").GetAttackModel(1).Duplicate();

            towerModel.GetAttackModel().RemoveWeapon(towerModel.GetAttackModel().weapons[0]);
            towerModel.GetAttackModel().AddBehavior(Game.instance.model.GetTowerFromId("SentryParagonGreen").GetAttackModel().GetBehavior<TargetSelectedPointModel>().Duplicate());

            towerModel.AddBehavior(laserSentry);
            towerModel.AddBehavior(greenChildSentry);

            towerModel.range = Game.instance.model.GetTowerFromId("SentryParagonGreen").range;
        }
    }
}
