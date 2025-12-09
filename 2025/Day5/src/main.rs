use std::fs;

fn read_from_file(file_path: &str) -> (Vec<(u64, u64)>, Vec<u64>) {
    let contents = fs::read_to_string(file_path).expect("Should have been able to read the file");
    let mut ranges: Vec<(u64, u64)> = vec![];
    for line in contents.lines() {
        if line.trim().is_empty() {
            break;
        }
        let parts: Vec<&str> = line.split('-').collect();
        let start: u64 = parts[0].trim().parse().unwrap();
        let end: u64 = parts[1].trim().parse().unwrap();
        ranges.push((start, end));
    }

    let mut ingredients: Vec<u64> = vec![];
    for line in contents.lines().skip(ranges.len() + 1) {
        if line.trim().is_empty() {
            break;
        }
        let value: u64 = line.trim().parse().unwrap();
        ingredients.push(value);
    }
    (ranges, ingredients)
}

fn main() {
    let file_path = "input.txt";
    let (ranges, ingredients) = read_from_file(file_path);
    println!("Part 1 Result: {}", part1(ranges, ingredients));
}

fn part1(ranges: Vec<(u64, u64)>, ingredients: Vec<u64>) -> u64 {
    // Implement part 1 logic here
    let fresh = ingredients
        .iter()
        .filter(|ingredient| ranges.iter().any(|(r1, r2)| (r1..&(r2+1)).contains(ingredient)))
        .count() as u64;
    fresh
}
