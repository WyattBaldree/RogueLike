public class _ideas
{
    /*  CLASS CARDS
     *  Class cards that you can buy or upgrade 1 at a time each gives 1 or more perks
     *  Some class cards require you to have other classes unlocked 
     *      - ie the knight class card requires that you have the warrior card
     *      - ie the spellsword class card requires that you have the warrior card and the apprentice card
     *  Your first class card is selected before the game begins
     *      - selected from the pool of class cards that have no requirements
     *      
     *  Fighter
     *  +1 strength
     *  obtain a rare weapon
     *  
     *  Apprentice
     *  +1 intelligence
     *  obtain a Lvl 1 spellbook
     *  
     *  Entrepreneur
     *  +1 dexterity
     *  obtain 100 gold (does not start with a weapon but can immediately buy one).
     *  
     *  Knight
     *  REQUIRES: Fighter
     *  +2 endurance
     *  obtain rare armor
     *  
     *  Warrior
     *  REQUIRES: Fighter
     *  +1 strength
     *  +1 endurance
     *  +1 damage with melee weapons
     *  
     *  Berserker
     *  Requires: Fighter
     *  +3 strength
     *  Beserk: You have anger issues.
     *  
     *  Smith
     *  REQUIRES: Fighter
     *  +1 strength
     *  obtain an epic piece of gear.
     *  
     *  Enchanter
     *  REQUIRES: Apprentice
     *  +2 Focus
     *  obtain 2(3?) scrolls of enchant item
     *  
     *  Arcane Smith
     *  REQUIRES: Smith, Enchanter 5 points
     *  +2 Strength
     *  +1 Focus
     *  obtain 2 legendary pieces of gear.
     *  
     *  Champion
     *  REQUIRES: Knight, Warrior 4 points
     *  +2 strength
     *  +1 endurance
     *  obtain a legendary weapon
     *  
     *  Alchemist
     *  REQUIRES: Apprentice
     *  +1 intelligence
     *  +1 Focus
     *  obtain and identify 3 potions
     *  
     *  Transmorger
     *  REQUIRES: Apprentice
     *  +2 Focus
     *  +obtain spell from transform spell list
     *  
     *  Werebeast
     *  REQUIRES: Transmorger, Berserker
     *  +2 strength
     *  +1 endurance
     *  Werebeast - You can transform into a Werebeast
     *  
     *  Beast King
     *  REQUIRES: Werebeast
     *  +2 strength
     *  +2 endurance
     *  +2 dexterity
     *  +You are permanently in beast form.
     *  
     *  Burglar
     *  REQUIRES: Entrepreneur
     *  +1 strength
     *  +1 dexterity
     *  +Crowbar - Can be used to open some locked chests and doors.
     *  
     *  Thief
     *  Requires: Entrepreneur
     *  +2 dexterity
     *  +Pickpocket - Quick action to steal from adjacent characters.
     *  
     *  Master Thief
     *  REQUIRES: Thief, Burglar
     *  +2 dexterity
     *  +1 strength
     *  +Lockpick - Unlock adjacent lock. (maybe a bank in town that can be robbed?)
     *  
     *  Merchant
     *  REQUIRES: Entrepreneur
     *  +2 charisma
     *  +400 gold
     *  
     *  Bard
     *  REQUIRES: Entrepreneur
     *  +1 charisma
     *  +1 dexterity
     *  obtain random instrument - instruments have effects that scale with charisma
     *  
     *  Spellsong
     *  REQUIRES: Bard, Apprentice
     *  +1 intelligence
     *  +1 charisma
     *  +1 dexterity
     *  instruments can be used as a quickaction
     *  
     *  Lord
     *  REQUIRES: Knight, Entrepeneur
     *  +2 Charisma
     *  +1 endurance
     *  +200 gold
     *  +Protector - Gain a loyal servant who will defend you till death
     *  
     *  Merchant King
     *  REQUIRES: Lord, Merchant
     *  +1000 gold
     *  +Protector - Gain a loyal servant who will defend you till death
     *  
     *  Monk
     *  Ninja
     *  Spellsword
     *  Spellthief
     *  Wizard
     *  Sage?
     *  Mage
     *  Sorcerer
     *  Man-At-Arms (buffs or enables dual wielding)
     *  
     *  enchantment to make a weapon return to your hand after throwing
     *  
     *  more quick actions. perhaps lower stat gains but give quick actions/spells/wands ect
     *  
     *  Attribute Levels
     *  STRENGTH
     *  Level 1 
     *      - you can properly wield daggers, slings, shortbows, and rapiers
     *  Level 2
     *      - +8 carrying capacity
     *      - you can now properly wield short swords, hatchets and crossbows
     *  Level 3
     *      - power attack - Spend Stamina to deal extra damage and break weak doors
     *      - you can now properly wield spears and longbows
     *  Level 4
     *      - +8 carrying capacity
     *      - you can now properly wield handaxes, maces, and straightswords
     *  Level 5
     *      - Shove - push an enemy forwards. Quick Action? One Handed Only?
     *      - you can wield longswords, halberds, and flails
     *  Level 6
     *      - +8 carrying capacity
     *      - you can wield greatswords and greataxes
     *  Level 7
     *      - Grapple - Spend Stamina to keep an enemy from moving. Quick Action? One Handed Only?
     *      - you can wield greathammers
     *  Level 8
     *      - +8 carrying capacity
     *  Level 9
     *      - Throw - Spend a lot of stamina to throw an adjacent enemy
     *  Level 10
     *      - Battle Rage - Your melee attacks do more damage!
     *  
     *  Endurance
     *  Level 1
     *      - 5 stamina
     *  Level 2
     *      - 10 stamina
     *  Level 3
     *      - 15 stamina
     *  Level 4
     *      - 20 stamina
     *  Level 5
     *      - 25 stamina
     *  Level 6
     *      - 30 stamina
     *  Level
     *  
     *  CONSTITUTION
     *  Level 1
     *      - 10 health
     *      - robes encumber less (magic, speed penalty)
     *  Level 2
     *      - 15 health
     *      - light armor encumbers less
     *  Level 3
     *      - 20 health
     *      - leather armor encumbers less
     *  Level 4
     *      - 25 health
     *  Level 5
     *      - 30 health
     *      - Tackle
     *  Level 6
     *      - 35 health
     *  Level 7
     *      - 40 health
     *  Level 8
     *      - 45 health
     *  Level 9
     *      - 50 health
     *  Level 10
     *      - 60 health
     *      
     *  Dexterity
     *  Level 10
     *      - Blur - You can use  2 quick actions per turn 
     * 
     * 
     * What increases stamina?
     * Tendril spell that has the same effect as grabbing but in aoe?
     * 
     * 
     * 
     * TO DO
     * make an input buffer so it feels less laggy.
     * 
     */
}
