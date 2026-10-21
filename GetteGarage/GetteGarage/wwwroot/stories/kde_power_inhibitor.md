See the code on <a href="https://github.com/gettecr/smart-kde-inhibitor" target="_blank" rel="noopener">GitHub</a>.

## The Problem

I switched my laptop to Linux and hit one annoying snag: it kept falling
asleep while I played piano into FL Studio over MIDI. Strawberry, my
music player, wouldn't keep it awake either. I could have changed the
power settings in two seconds. Instead I spent about fifteen hours
writing scripts so I'd never have to think about it again.

## What It Does

The inhibitor blocks system suspend while an app is active, but still
lets the screen turn off when the session is idle or locked. It has two
modes:

- **Playing:** for media players. Sleep is blocked only while music is
  actually playing.
- **Always:** for DAWs and long jobs. Sleep is blocked for as long as the
  app is open.

## Small Pieces, Loosely Joined

The core logic lives in one reusable script, and each app gets a thin
wrapper that launches it and starts the inhibitor alongside. Adding a new
app means copying a few lines, not rewriting anything.
