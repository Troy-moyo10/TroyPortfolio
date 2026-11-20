// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.documentElement.classList.add("js");

document.addEventListener("DOMContentLoaded", () => {
    if (window.lucide) window.lucide.createIcons();

    const counters = document.querySelectorAll(".count-up[data-count]");
    const revealItems = document.querySelectorAll("[data-scroll-reveal]");
    const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    if (counters.length) {
        const showFinalValues = () => counters.forEach((counter) => {
            counter.textContent = Number(counter.dataset.count).toLocaleString();
        });

        if (prefersReducedMotion) {
            showFinalValues();
        } else {
            const animateCounter = (counter) => {
                const target = Number(counter.dataset.count);
                const duration = 1400;
                const startTime = performance.now();

                const step = (now) => {
                    const progress = Math.min((now - startTime) / duration, 1);
                    const easedProgress = 1 - Math.pow(1 - progress, 3);
                    counter.textContent = Math.round(target * easedProgress).toLocaleString();
                    if (progress < 1) requestAnimationFrame(step);
                };

                requestAnimationFrame(step);
            };

            const counterObserver = new IntersectionObserver((entries, currentObserver) => {
                entries.forEach((entry) => {
                    if (!entry.isIntersecting) return;
                    animateCounter(entry.target);
                    currentObserver.unobserve(entry.target);
                });
            }, { threshold: 0.5 });

            counters.forEach((counter) => counterObserver.observe(counter));
        };
    }

    if (!revealItems.length) return;
    if (prefersReducedMotion || !("IntersectionObserver" in window)) {
        revealItems.forEach((item) => item.classList.add("scroll-visible"));
        return;
    }

    const revealObserver = new IntersectionObserver((entries, currentObserver) => {
        entries.forEach((entry) => {
            if (!entry.isIntersecting) return;
            entry.target.classList.add("scroll-visible");
            currentObserver.unobserve(entry.target);
        });
    }, { threshold: 0.15 });

    revealItems.forEach((item, index) => {
        item.style.setProperty("--reveal-delay", `${(index % 3) * 120}ms`);
        revealObserver.observe(item);
    });
});
