var FibonacciSequence = function () {

    this.term = function (n) {
        if (n < 0)  { return undefined; }
        if (n == 0) { return 1;}
        if (n == 1) { return 1;}

        return this.term(n - 1) + this.term(n - 2);
    }

    this.iterator = function () {
        var i = 0, that = this;
        return {
            next: function () {
                return that.term(i++);
            }
        };
    }
};