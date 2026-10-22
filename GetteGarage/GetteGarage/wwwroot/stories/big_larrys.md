Check out the Dev_log page for more details on this post: [I made and published a game in six weeks!](/blog/i-made-and-published-a-game-in-6)

Play the game on itch! [Big Larry's Beach Baseball](https://mosswaffle.itch.io/big-larrys-beach-baseball)

## The Spark

Playing Mario Party with friends, I realized I always gravitate toward the baseball minigame. I love a good batting cage, but I wanted
more interesting things to hit. That idea became Big Larry's Beach Baseball.

## Six Weeks, One Beach

I built this for Tiny Mass Games, a Massachusetts based collective that
gives small teams two months to make and publish something. Travel plans
cut my window down to six weeks, so I kept the scope tight from day one:
one-touch aiming, mobile-friendly, single-screen gameplay, 2D art I could
finish in time. I used Godot for the prototyping.

A half-serious brainstorm around "Solar Punk" somehow turned into "sun,"
then "beach," and Big Larry was born: a big, relaxed guy who'd rather
swing a bat at beach balls than play actual baseball.

## Building It Out

Once Larry existed, everything else scaled around him: bouncing beach
balls, kites, sinking boats, and a shop to unlock new hats and bats. A
full week went into art and color alone, aiming for something bright and
a little oversaturated. Sound came together last, a mix of a licensed soundtrack and homemade SFX stitched
together in Audacity.

## Feedback Loop

Boston Indies Demo Night gave me a room of players to test the timing
feel. A trip home let my mom (very much not a gamer) playtest it too,
and her notes on the swing timing turned out to be some of the most
useful feedback I got. I also had to write a small script to silently
preload every shader and sound before gameplay starts, just to stop the
web build from stuttering the first time each one loaded in.

## Ship It

Four levels, a shop, and a soundtrack later, I called it done and
published it the same day. There's a long list of things I'd still love
to add (more customization, maybe a kraken boss level) but for six
weeks, I'm happy with where Big Larry landed.
