Read the full write-up in the dev log: [VineScribe: Digitizing Handwritten Wine Tasting Notes with AI](/blog/vinescribe-digitizing-handwritten)

See the code on <a href="https://github.com/gettecr/vinescribe" target="_blank" rel="noopener">GitHub</a>.

## The Problem

My wife and I have been jotting down wine tasting notes for about six
years. We're not sommeliers (one early entry reads "cork smells like a
crappy hamburger"), but we have fun with it. The trouble is finding
anything. To check whether we've had a wine before, we flip through the
whole notebook, and we often forget a wine exists at all.

## Email Is the Interface

VineScribe is built around a dedicated email inbox. Photograph a
notebook page, email it with "page" in the subject, and the notes get
transcribed into a searchable database. Email a photo of a wine label
with "check" in the subject, and you get a reply saying whether we've
tried it, what we thought, or what similar wines we've liked.

The inbox works as an offline queue. A Raspberry Pi checks for new
messages every so often, processes them, and replies, so there's no web
server to run. It isn't instant, but it's light, and getting a reply
email is oddly nice.

## How It Works

Gemini reads the handwriting, including my scrawl, and returns
structured data. For label checks, the whole tasting database goes into
the model's context window so it can look for matches and similar wines.
It takes a quick phone photo, no perfect scans needed, which is
convenient because wine is usually involved.
