# ByGameBuild - Game Design Document
[French version of the GDD (to be written)]()

## Game Overview

*Note: All images used in this document are for illustrative purposes only. They do not represent the final state of the game.*

<u>**Game Title:**</u> Baby Yoda’s Game (aka ByGame)  
<u>**Genre:**</u> One-Button Arcade Game  
<u>**Platform:**</u> PC  
<u>**Target Audience:**</u> Star Wars fans, arcade game enthusiasts  
<u>**ESRB Rating (Estimated):**</u> E10+ / T (Teen)  
<u>**Estimated Playtime:**</u> 30–45 minutes  
<u>**Camera Perspective:**</u> First-person  

### Introduction, Story and universe

Baby Yoda’s Game (aka ByGame) is a one-button arcade game inspired by the Disney+ show The Mandalorian.  
The game takes place aboard the Razor Crest, the flagship of the Mandalorian and his protégé, Grogu — also known as Baby Yoda.  
Throughout the show, Baby Yoda is often seen trying to unscrew and play with a small ball from the ship’s control panel.  
That is the player’s objective in this game: unscrew the ball without getting caught by Mando before the journey ends.  

### Unique Selling Point

1. "Play as Grogu in a forbidden stealthy mini-game moment"
Experience a charming and mischievous moment from The Mandalorian from Baby Yoda's perspective in a fast-paced, one-button stealth arcade game.

2. "A one-button stealth challenge full of tension and cuteness"
Master the art of subtle sabotage as Grogu in a unique mix of tension and cuteness — all with just one button.

3. "A Star Wars parody game mixing humor, stealth and baby-level rebellion"
A light-hearted arcade experience blending Star Wars fanservice with sneaky gameplay and a dose of Baby Yoda charm.

### References

- The mandalorian / Star wars shows as the characters and universe reference
- Game doc geraud
- Menus/UI visuals Star wars Outlaws by Ubisoft
- Paper Mario / Night in the Woods (for stylized 2D character animation)
- Among Us (for minimalistic sci-fi interiors)
- Children's book illustrations (for softness and clarity of forms)

### Artistic direction

#### Visual

<u>**Visual Style:**</u>
The game features a stylized 2D art style with clean lines and soft shading, evoking a playful yet slightly tense atmosphere. The visuals are cartoon-like, with exaggerated proportions and smooth animations to emphasize Grogu’s cuteness and expressiveness. Despite the simplified look, the art direction remains faithful to the Star Wars universe in its design language.
We want for the player to see the world through grogu's eyes.

<u>**Color Palette:**</u>
Muted greys and browns dominate the Razor Crest's interior, reflecting its rugged, industrial feel. Grogu is highlighted with warmer, more saturated tones (green, beige, brown) to draw the player's eye. Important interactive elements — like the control panel ball — use bright accents (e.g., silver, blinking red/blue lights) to enhance visibility and focus.

<u>**Character Design:**</u>
Grogu is presented in a chibi-inspired design, with large eyes and fluid, playful animations that highlight his curiosity.  
Mando appears only in partial views — his legs, cape, or silhouette in the background — helping build tension and giving the player a sense of being watched.

<u>**UI/UX:**</u>
The objective is to have a diegetic interface — no external HUD or indicators. Feedback is conveyed through visual cues (Grogu’s facial expressions, light flashes, environmental shaking) and subtle audio hints. This reinforces immersion and maintains the first-person illusion from Grogu's perspective.
For the moment, we use classic HUD to give player the needed informations.

#### Audio

TODO

### Characters

#### Grogu aka Baby yoda

Baby yoda is a character from the Star Wars Disney+ original television series The Mandalorian and The Book of Boba Fett. He is an infant member of the same species as the Star Wars characters Yoda and Yaddle, with whom he shares a strong ability in the Force. In the series, the protagonist known as "the Mandalorian" is hired to track down and capture Grogu for a remnant of the fallen Galactic Empire, but instead, he becomes his adoptive father and protects him from the Imperials.

#### Din Djarin (aka The Mandalorian)

Din Djarin is a stoic and disciplined bounty hunter who rarely removes his helmet and lives by the Mandalorian code. As Grogu’s reluctant guardian, he balances toughness with a quiet sense of protectiveness. In the game, he serves as a looming authority figure — ever-present, unpredictable, and the main obstacle to Grogu’s mischief.

## Gameplay

The player can choose to use a mouse, keyboard, or gamepad to interact with the game and its UI.  
During gameplay, the player uses the Force through a specific keybind. The goal is to keep an indicator within a certain range in order to unscrew the ball.  
If the player is caught, Din Djarin reverses the player’s progress.  
Occasionally, a special range may appear, giving the player an "unscrew boost" to recover lost progress.  

TODO: Gameflow

## UI / UX

### Main menu / score display

From the main menu, the player can:  

- Start a new game
- Access the settings panel
- Exit the game

By default, the main menu also displays:  

- The top scores achieved by other players
- The local player's personal best

In the background, a chibi version of the Razor Crest floats across the screen, with Grogu and Din Djarin’s heads visible.

### Settings

The settings menu allows the player to customize visual, audio, and control preferences.

**Visual Settings**

- Screen resolution
- Refresh rate

After changing a visual setting, a confirmation popup appears to validate or revert the changes.

**Audio Settings**

- Master volume
- Music volume
- SFX volume

Each volume setting has a sound test button for instant feedback.

**Keybind Settings**

- Mouse controls
- Keyboard controls
- Gamepad controls

If the player attempts to rebind a key using an unsupported device, a popup will notify them and cancel the operation.

A "Restore Default Settings" button is also available in all categories.

### Start game

When a player starts a game, he can choose between 3 different difficulties:

- Padawan for easy
- Jedi knight for normal
- Master for hard

The game duration, Mando checks, and others mutators are applied Depending on the difficulty

Then the player starts the game. The base game is in normal difficulty.

### Loading

The loading screen shows tips and tricks about the gameplay. It will wait for the player to press a key before exiting the loading screen.

### Pause menu

The pause menu is triggered by ESC key and keyboard or Start key on Gamepad

It gives the payer to be able to:
- Resume the paused game
- Restart the game with/without switching the game difficulty
- Go to the main menu
- Personalize some settings
- Leave the game

### Game HUD