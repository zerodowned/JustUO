using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an orcish corpse")]
    public class OrcChopper : BaseCreature
    {
        [Constructable]
        public OrcChopper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Orc Chopper";
            Body = 7;
            BaseSoundID = 0x45A;
            Hue = 0x96D;

            SetStr(147, 245);
            SetDex(101, 135);
            SetInt(86, 110);

            SetHits(97, 139);

            SetDamage(4, 13);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 25, 30);

            SetSkill(SkillName.MagicResist, 60.1, 85.0);
            SetSkill(SkillName.Swords, 75.1, 90.0);
            SetSkill(SkillName.Tactics, 60.1, 85.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 54;

            PackItem(new Log(Utility.RandomMinMax(1, 10)));
            PackItem(new Board(Utility.RandomMinMax(10, 20)));
            PackItem(new ExecutionersAxe());

            // TODO: Skull?
            switch (Utility.Random(7))
            {
                case 0:
                    PackItem(new Arrow());
                    break;
                case 1:
                    PackItem(new Lockpick());
                    break;
                case 2:
                    PackItem(new Shaft());
                    break;
                case 3:
                    PackItem(new Ribs());
                    break;
                case 4:
                    PackItem(new Bandage());
                    break;
                case 5:
                    PackItem(new BeverageBottle(BeverageType.Wine));
                    break;
                case 6:
                    PackItem(new Jug(BeverageType.Cider));
                    break;
            }

            if (Core.AOS)
                PackItem(Loot.RandomNecromancyReagent());
        }

        public OrcChopper(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType
        {
            get { return InhumanSpeech.Orc; }
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override int Meat
        {
            get { return 1; }
        }

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.SavagesAndOrcs; }
        }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(2))
            {
                default:
                case 1:
                    return WeaponAbility.WhirlwindAttack;
                case 2:
                    return WeaponAbility.CrushingBlow;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new DoubleAxe());

            if (Core.ML)
            {
                if (Utility.RandomDouble() < 0.05)
                    c.DropItem(new StoutWhip());
            }

            if (Utility.RandomDouble() < 0.1)
                c.DropItem(new EvilOrcHelm());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager, 2);
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            var item = aggressor.FindItemOnLayer(Layer.Helm);

            if (item is OrcishKinMask)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}