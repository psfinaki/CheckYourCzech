module Client.UiTests.WebElementHelper

open OpenQA.Selenium

let findByTag tag (e: IWebElement) = e.FindElements(By.TagName tag)

let getAttribute name (e: IWebElement) = e.GetAttribute name

let isEnabled (e: IWebElement) = e.Enabled

let getClass (e: IWebElement) = e |> getAttribute "class"
