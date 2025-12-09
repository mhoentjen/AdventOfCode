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
    println!("Part 2 Result: {}", part2(ranges));
}

fn part1(ranges: Vec<(u64, u64)>, ingredients: Vec<u64>) -> u64 {
    let fresh = ingredients
        .iter()
        .filter(|ingredient| {
            ranges
                .iter()
                .any(|(r1, r2)| (r1..&(r2 + 1)).contains(ingredient))
        })
        .count() as u64;
    fresh
}

fn part2(ranges: Vec<(u64, u64)>) -> u64 {
    let mut sorted_ranges = ranges.clone();
    sorted_ranges.sort_by_key(|&(start, _)| start);

    let mut merged_ranges: Vec<(u64, u64)> = vec![];
    for range in sorted_ranges {
        if let Some(last) = merged_ranges.last_mut() {
            if ranges_overlap(last, &range) {
                last.1 = std::cmp::max(last.1, range.1);
            } else {
                merged_ranges.push(range);
            }
        } else {
            merged_ranges.push(range);
        }
    }
    merged_ranges.iter().map(|(r1, r2)| r2 - r1 + 1).sum()
}

fn ranges_overlap(range1: &(u64, u64), range2: &(u64, u64)) -> bool {
    !(range1.1 < range2.0 || range2.1 < range1.0)
}

