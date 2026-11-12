<div align="center">

# Let's School Trainer

**A small trainer for Let's School: set money, knowledge points, teacher and staff stats, unlock furniture, and more.**

One `.exe` for the trainer and one small plugin file for the game. The only other thing you need is BepInEx.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![Windows](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D4)
![UI](https://img.shields.io/badge/UI-WPF%20%2B%20WPF--UI-6A2FA0)
![Game](https://img.shields.io/badge/game-Let%27s%20School-4C9F38)
![Release](https://img.shields.io/github/v/release/fathun12/LetsSchool-Trainer?label=release)

<img src="docs/screenshot-1.png" alt="Let's School Trainer" width="420">

> Type a value, press Set. The game does the rest.

</div>

---

## Features

| Feature | What it does |
|---|---|
| School Funds | Sets the school's money to the value you enter. |
| All Knowledge Points | Sets Arts, Science, Culture and Sports points to the same value. |
| Knowledge by category | Sets each of the four categories separately. Leave a box blank to keep that category unchanged. |
| All Teacher Stats | Sets every ability of every teacher to one value. |
| Single teacher | Pick one teacher from a list and set Teaching, Research, Managing and Cultivating. Blank boxes stay unchanged. |
| Club Strength | Sets club power for every student who is in a club. |
| Staff Mutation | Sets abilities for all employees and grants every certificate. |
| Support Capability | Sets medical, cooking, sales and security ability for all staff. |
| Single staff member | Pick one person, choose a role and a skill value. The matching certificate is granted too. |
| Unlock All Furniture | Unlocks every furniture blueprint, with no requirements. |
| Clean Invalid Ground | Removes broken ground style IDs that cause blurry rendering in the build menu. |

Money and knowledge points accept whole numbers up to 2,147,483,647. The other boxes also accept decimals.

## How it works

The trainer does not read or write game memory. It has two parts:

1. **The trainer** (`LetsSchool Trainer.exe`) writes your command into a small text file in `C:\Users\Public`.
2. **The plugin** (`ModGuruJenius.dll`) runs inside the game through BepInEx. It checks that file about twice a second and carries out each command with the game's own code.

Because the plugin uses the game's own code instead of memory addresses, it is less likely to break than an address-based trainer. A large game update can still break it.

The status line at the bottom shows what happened. "Sent to the game" means the plugin picked the command up. The result itself shows in the game.

## Requirements

- Windows 10 or 11 (64-bit)
- Let's School (Steam)
- [BepInEx](https://github.com/BepInEx/BepInEx/releases) (the Mono build) installed in the game folder
- The plugin file `ModGuruJenius.dll` from the Releases page

Nothing else to install. The .NET runtime is included in the exe.

## Installation

1. Download `LetsSchool Trainer.exe` and `ModGuruJenius.dll` from the [Releases](../../releases) page.
2. Extract BepInEx into the game folder, next to `LetsSchool.exe`, then start the game once so BepInEx creates its folders.
3. Close the game and copy `ModGuruJenius.dll` into `BepInEx\plugins`.

## Usage

1. Start the game and open a save.
2. Run `LetsSchool Trainer.exe`.
3. Type a value in a row and press **Set**, or press the button on an action row.
4. For one teacher or one staff member, press **Load list**, pick a person from the dropdown, then press **Apply**.

Back up your save files before using the trainer.

## Troubleshooting

| Problem | Fix |
|---|---|
| "The game did not pick it up" | Start the game and check that `ModGuruJenius.dll` is in `BepInEx\plugins`. |
| "No answer from the game" when loading a list | Same as above. Also open a save first, so there are teachers and staff to list. |
| A list loads but is empty | Open a save first, then press **Load list** again. |
| A value did not change in the game | Check the BepInEx log in the game folder for an error from the plugin. |
| Windows SmartScreen warning | The exe is not code-signed, so Windows may warn about it. Only download it from this repository's Releases page. |

## Disclaimer

Use at your own risk. Intended for single-player use. This project is not affiliated with or endorsed by the developers or publishers of Let's School. All trademarks belong to their owners.