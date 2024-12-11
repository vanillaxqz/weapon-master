# Weapon Master
VR app that combines interactive lessons on Japanese weapons with game-style combos,
offering an engaging, gamified learning experience.

### Team members:
- Andrei Chiriac
- Iulian Roman

[State of the Art](https://github.com/vanillaxqz/weapon-master/blob/main/State%20of%20the%20art.pdf)\
[Main Modules](https://github.com/vanillaxqz/weapon-master/blob/main/Main%20Modules.pdf)

Meta Quest 2 code  1WMHHA73JE3284

# Progress:
## Week 1:
[Video](https://youtu.be/3beTF6MW9gQ)
- Modeled the map: arena, trees, tori, stairs, floor
- Modeled wooden katana
- Imported free assets: skybox, dummy, naginata

## Week 2:
[Video](https://youtu.be/jLjotvfpmo4)
- UV unwrap map + added map textures
- Reduced polygon count of tree bases
- Reduced polygon count of floor
- Added new naginata and tree assets + remade their textures
- Added BGM to main scene
- Added title screen

## Week 3:
[Video](https://youtu.be/zHCQWqmqmqA)
- Volume slider + saved volume preference
- Persistent bgm between title screen and main scene
- VFX/SFX for real katana + naginata On Collision
- Weapons feel better + gravity
- Locomotion + jump
- Weapons grab interactable

## Week 4:
[Video](https://youtu.be/vnuLdA4G3nw)
- Further improved feeling of the weapons
- Collider only for the sharp side of the blade
- Floating weapon selection
- New dummy collider
- Naginata two handed grip
### What should the fighting system look like (not implemented yet):
#### Single move:
- animation 
- guideline
- start point
- endpoint
- time to execute() - how long are we in collision
- accuracy() - start and end point of collision vs guideline, distance from it, etc
#### Combat Moves:
- singlemove[] moves;
- timebetween moves;
- aggregate accuracy()