Read the full write-up in the dev log: [Using Gemini to Help Analyze the Collection for my Local Library](/blog/using-gemini-to-help-analyze-the)

Explore the analysis on <a href="https://github.com/gettecr/WestminsterAnalysis" target="_blank" rel="noopener">GitHub</a>.

## The Question

My wife is a librarian, and her library has wanted to dig into its
collection for years: what actually gets borrowed, and does the shelf
match what people want? The catch was that the collection wasn't
labeled in a way that answered that. "Fiction" is just Fiction. They
needed better labels, cheaply.

## Exploring ~50,000 Items

I started where I usually do with a new dataset: poke around the numbers,
check the categories, and flag anything weird. Since I'd never worked
with library data, my wife served as subject matter expert and decoded
columns like ILL (Interlibrary Loan, not a "sick" record) for me. Then I
compared each section's share of the collection to its share of
circulation.

## Letting AI Label the Fiction

To split Fiction into genres, I gave Gemini the title and author of every
fiction book and asked it to pick one genre from a fixed list. About
7,500 books and an hour and a half later, the whole section was labeled.
I spot-checked a sample against Goodreads, and the labels were good
enough for aggregate analysis, though a few are debatable.

## What Turned Up

- New books circulate far more than existing ones.
- Large print is surprisingly popular.
- Kids borrow more than anyone.
- Thrillers and romance dominate the fiction shelves.
- Adult nonfiction underperforms for its size (and, I'm told, lives on a
  floor where the lights shut off after three minutes).

My wife says none of this surprised her.
