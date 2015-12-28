///<reference path="~/dependencies/jasmine.js"/>
///<reference path="~/sources/FibonacciSequence.js"/>

describe("FibonacciSequence", function () {
    var fib = new FibonacciSequence();

    it("first two terms are one", function () {
        expect(fib.term(0)).toBe(1);
        expect(fib.term(1)).toBe(1);
    });

    it("each term is the sum of the previous two", function() {
        expect(fib.term(2)).toBe(2);
        expect(fib.term(3)).toBe(3);
        expect(fib.term(4)).toBe(5);
        expect(fib.term(5)).toBe(8);
    });

    it("is not defined for negative terms", function() {
        expect(fib.term(-2)).toBeUndefined();
    });

    it("can be iterated through", function () {
        var seq = fib.iterator();
        expect(seq.next()).toEqual(1);
        expect(seq.next()).toEqual(1);
        expect(seq.next()).toEqual(2);
        expect(seq.next()).toEqual(3);
        expect(seq.next()).toEqual(5);
        expect(seq.next()).toEqual(8);
    });

    it("can have multiple independent iterators", function () {
        var seq = fib.iterator();
        expect(seq.next()).toEqual(1);
        expect(seq.next()).toEqual(1);
        expect(seq.next()).toEqual(2);

        var seq2 = fib.iterator();
        expect(seq2.next()).toEqual(1);

        expect(seq.next()).toEqual(3);
        expect(seq.next()).toEqual(5);
    });
});