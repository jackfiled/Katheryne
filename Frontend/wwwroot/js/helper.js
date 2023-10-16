function scrollToSection() {
    let element = document.getElementById("chat-scroll-zone")
    element.scrollTo({
        top: element.scrollHeight - element.offsetHeight,
        behavior: "smooth"
    })
}