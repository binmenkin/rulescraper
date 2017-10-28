# Rulescraper
A tool for getting data out of web pages.

## Installation
Download the project files. Import the project into Visual Studio and run it or use the compiled program from the folder `rulescraper\bin\Debug (rulescraper.exe)`.

## Usage
1. Run the program.
2. Enter a website address.
	1. Add attributes if necessary.
3. Click **Add Rules**.
	1. Enter your rule name.
	2. Enter your data pattern.
	3. Click **OK**.
4. Repeat Part.3 if necessary.
5. Click **Scrape**.
	1. Click **Current Page** to scrape the current page from the browser if necessary.

### Data patterns (rules)
Use special characters to scrape data correctly:
- \* (empty space or string of any length).
- ? (any single character).
- \# (equivalent \*, data to scrape).

#### Examples
```html
<div class="div1c">item1</div>
<div class="div2c" id="div2i">item2</div>
<div class="div3c">item3</div>
<div class="div4">item4</div>
```
+ Rule: `<div class="div1c">#<`
	+ Result: `item1`
+ Rule: `<div*>#<`
	+ Result: `item1, item2, item3, item4`
+ Rule: `<div>#<`
	+ Result: `-`
+ Rule: `<div class="div?c">#<`
	+ Result: `item1, item3`
+ Rule: `<div class="#">`
	+ Result: `div1c, div3c, div4`

### Additional attributes
Attributes are specified in the URL text box. Common format: `mysite.com {attribute}`. Available attributes:
+ *click* (clicks on a given selector after scraping).
	+ `click:<selector>`
	+ `click:<delay>:<selector>`
	+ `click:<delay>:<maxClicks>:<selector>`
+ *click-expand* (clicks on a given selector before scraping, same formats as for `click`).
+ *iter* (iterates through pages).
	+ `iter:<startPage>`
	+ `iter:<startPage>:<endPage>`
	+ `iter:<startPage>:<endPage>:<step>`
+ *item* (scrapes data from individual item pages).
	+ `item:<itemLinkRule>`
	+ `item:<itemLinkPrefix>:<itemLinkRule>`
	
Examples with attributes can be found in the folder `examples`.
